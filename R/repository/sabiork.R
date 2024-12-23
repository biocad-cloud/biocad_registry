#' imports enzymatic kinetics model
#' 
#' @param repo a character vector of the xml data files for the enzymatic kinetics data
#' 
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
                let substrate = xrefs[[args$S]]; 

                if (length(substrate)>0) {
                    substrate = db_xrefs 
                    |> where(type=4, xref in substrate) 
                    |> group_by("obj_id") 
                    |> order_by("count",desc=TRUE) 
                    |> find("obj_id","count(*) as count")
                    ;

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
                    uniprot = uniprot_id,
                    ec_number = [rxn]::Ec_number,
                    json_str = JSON::json_encode(rxn),
                    note = [rxn]::reaction
                );

                str(enzymes);
                str(args);
                str(xrefs);
                str(rxn);
            }
        }

        stop();
    }
}