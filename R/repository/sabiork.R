#' Import enzymatic kinetics models from SABIO-RK
#'
#' This function imports enzyme kinetic data from SABIO-RK SBML files into a biocad 
#' registry database, handling model parsing, parameter extraction, and database 
#' relationship mapping.
#'
#' @param biocad_registry A biocad database registry connection object
#' @param repo Input specification for kinetic data. Can be either:
#'             - Character vector of SBML file paths
#'             - Pipeline enumerator for stream processing
#'
#' @return Invisibly returns NULL. Mainly used for populating the biocad registry 
#'         database with kinetic data.
#' 
#' @details The function performs these key operations:
#' 1. Processes SBML files from SABIO-RK database
#' 2. For each biochemical reaction:
#'    - Extracts kinetic parameters and enzyme information
#'    - Links reactions to existing substrate references via database cross-references
#'    - Stores complete kinetic models with environmental conditions (pH, temperature)
#'    - Maintains traceability through SABIO-RK identifiers
#' 3. Handles duplicate prevention using native database checks
#'
#' @section Database Integration:
#' - kinetic_laws: Main storage for kinetic parameters and model metadata
#' - db_xrefs: Cross-reference system for substrate identification
#'
#' @section Data Processing Workflow:
#' 1. SBML file parsing and validation
#' 2. Reaction parameter extraction:
#'    - Kinetic constants (lambda)
#'    - Experimental conditions (pH, temperature, buffer)
#'    - Enzyme associations (UniProt IDs, EC numbers)
#' 3. Substrate identification through cross-reference matching
#' 4. JSON-structured storage of raw reaction data
#'
#' @note Important implementation details:
#' - Requires valid SBML files from SABIO-RK database
#' - Depends on biosystem package for SBML parsing
#' - Uses JSON encoding for complex parameter structures
#' - Auto-skips invalid/malformed SBML files
#' - Substrate matching uses type=4 cross-references (verify database schema)
#' - Progress tracking only available for file vector inputs
#'
#' @examples
#' \dontrun{
#' # Import single SBML file
#' imports_sabiork(biocad_registry, "path/to/kinetics.xml")
#'
#' # Batch import multiple files
#' imports_sabiork(biocad_registry, c("file1.xml", "file2.xml")) 
#' }
const imports_sabiork = function(biocad_registry, repo) {
    imports "sabiork" from "biosystem";

    let kinetic_laws = biocad_registry |> table("kinetic_law");
    let db_xrefs = biocad_registry |> table("db_xrefs");

    for(let filepath in tqdm(repo)) {
        let sbml = parseSbml(filepath |> readText());

        if ([sbml]::empty) {
            next;
        } else {
            sbml <- unset_sbml(sbml);
        }
    
        for(let rxn in sbml) {
            let kinetic_id = [rxn]::SabiorkId;

            if (!(kinetic_laws |> check(db_xref = kinetic_id))) {
                let enzymes = [rxn]::enzyme;
                let args = [rxn]::parameters;
                let xrefs = [rxn]::xref;
                let uniprot_id = [rxn]::uniprot_id;
                let substrate = __find_substrate_id(args, xrefs); 

                if (length(substrate)>0) {
                    substrate = db_xrefs 
                    |> where(type=4, xref in substrate) 
                    |> group_by("obj_id") 
                    |> order_by("count",desc=TRUE) 
                    |> find("obj_id","count(*) as count")
                    ;

                    # stop(get_last_sql(db_xrefs));

                    if (length(substrate) ==0) {
                        substrate <- 0;
                    } else {
                        substrate <- substrate$obj_id;
                    }
                } else {
                    substrate <- 0;
                }

                kinetic_laws |> add(
                    db_xref = kinetic_id,
                    params = JSON::json_encode(args ),
                    lambda = [rxn]::lambda,
                    temperature = [rxn]::temperature,
                    pH = [rxn]::PH,
                    buffer = [rxn]::buffer,
                    substrate_id = substrate,
                    uniprot = uniprot_id || "-",
                    ec_number = [rxn]::Ec_number,
                    json_str = JSON::json_encode(rxn),
                    note = [rxn]::reaction
                );
            }
        }
    }
}

const __find_substrate_id = function(args, xrefs) {
    let keys = names(args);

    for(let chr in ["S","A","B","C"]) {
        if (any(chr == keys)) {
            let key = args[[chr]];
            return(xrefs[[key]]);
            break;
        }
    }

    NULL;
}