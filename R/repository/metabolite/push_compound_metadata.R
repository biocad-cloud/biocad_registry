#' Save compound metadata to a biocad registry
#' 
#' This function stores compound metadata into a biocad registry, updating 
#' several related tables including sequence_graph, db_xrefs, and synonyms.
#'
#' @param biocad_registry An object representing the biocad registry database 
#'   where metadata will be stored. Must provide access to tables 
#'   "sequence_graph", "db_xrefs", and "synonym".
#' @param compound A list containing the compound's metadata. Must follow the 
#'   structure specified in the Details section.
#' @param mol A molecule record from the current `biocad_registry`. Must contain 
#'   a character `id` property to associate metadata with this molecule.
#' 
#' @details 
#' The `compound` parameter must be a structured list with the following fields:
#' 
#' ```r
#' list(
#'    ID = "unique-id",               # Required: Unique identifier (character)
#'    formula = "formula-string",     # Required: Chemical formula (character)
#'    exact_mass = 0,                 # Required: Exact mass (numeric)
#'    name = "name_string",           # Required: Common name (character)
#'    IUPACName = "name_string",      # Required: IUPAC name (character)
#'    description = "description_string",  # Optional: Description (character)
#'    synonym = c("synonyms", "names"),    # Optional: Character vector of synonyms
#'    xref = list(                    # Optional: Cross-references to external databases
#'        chebi = "",                 # Use "" or omit if unavailable
#'        KEGG = "",
#'        pubchem = "",
#'        HMDB = "",
#'        Wikipedia = "",
#'        lipidmaps = "",
#'        MeSH = "",
#'        MetaCyc = "",
#'        foodb = "",
#'        CAS = "",
#'        InChIkey = "",              # Note: InChIkey/InChI/SMILES are stripped from xref
#'        InChI = "",                 #   and processed separately
#'        SMILES = ""
#'    )
#' )
#' ```
#' 
#' ### Processing Logic:
#' 1. ​**Structure Handling**:
#'    - SMILES strings are parsed (non-strictly) and stored in `sequence_graph` 
#'      if valid. Invalid SMILES are skipped.
#'    - InChIkey, InChI, and SMILES fields are removed from `xref` to avoid 
#'      duplication; SMILES is explicitly stored in `sequence_graph`.
#' 
#' 2. ​**Cross-References**:
#'    - Valid `xref` entries (non-empty strings) are added to `db_xrefs`, 
#'      linked to the molecule's `id`. Each entry is checked for duplicates 
#'      before insertion.
#' 
#' 3. ​**Synonyms**:
#'    - Synonyms are stored in the `synonym` table with MD5 hashes to prevent 
#'      duplicates. Only English (`lang = 'en'`) synonyms are supported.
#' 
#' @section Side Effects:
#' - Modifies tables in `biocad_registry`: 
#'   - Adds entries to `sequence_graph` for valid SMILES.
#'   - Adds cross-references to `db_xrefs`.
#'   - Adds synonyms to `synonym`.
#' - Duplicate entries are skipped using checks (e.g., `db_xrefs |> check(...)`).
#' 
#' @return Invisibly returns `NULL`. This function is called for its side effects.
#' 
#' @examples
#' \dontrun{
#' # Assume `registry` is a pre-configured biocad_registry
#' compound <- list(
#'   ID = "C00001",
#'   formula = "H2O",
#'   exact_mass = 18.0106,
#'   name = "Water",
#'   IUPACName = "Oxidane",
#'   synonym = c("H2O", "Dihydrogen monoxide"),
#'   xref = list(pubchem = "962", KEGG = "C00001")
#' )
#' mol <- list(id = "molecule_123")  # Pretend this is from biocad_registry
#' 
#' __push_compound_metadata(registry, compound, mol)
#' }
#' 
#' @export
const __push_compound_metadata = function(biocad_registry, compound, mol) {
    let seq_graph = biocad_registry |> table("sequence_graph");
    let db_xrefs  = biocad_registry |> table("db_xrefs");
    let xrefs     = compound$xref;
    let smiles    = gsub(xrefs$SMILES, "%","") |> trim('" '); 
    let term_metabolite = biocad_registry::metabolite_term(biocad_registry);

    # removes all molecular strucutre data
    xrefs$InChIkey <- NULL;
    xrefs$InChI    <- NULL;
    xrefs$SMILES   <- NULL;
    xrefs$extras   <- NULL;
    
    let met_struct = {
        if (nchar(smiles) == 0) {
            NULL;
        } else {
            SMILES::parse(smiles, strict = FALSE);
        }
    };

    # null means parser error
    if (!is.null(met_struct)) {
        # let atoms_vec = SMILES::atoms(met_struct);

        # atoms_vec = atoms_vec 
        # |> groupBy("group") 
        # |> lapply(grp -> sum(grp$links))
        # ;

        # let embedding = bencode( names(atoms_vec));
        # atoms_vec = as.numeric(unlist(atoms_vec));
        # atoms_vec = base64(packBuffer(atoms_vec ) |> zlib_stream());

        if (!(seq_graph |> check(molecule_id = mol$id))) {
            seq_graph |> add(
                molecule_id = mol$id,
                sequence    = smiles,
                # seq_graph   = atoms_vec,
                # embedding   = embedding,
                morgan = "",
                hashcode    = md5(smiles)
            );
        }
    }

    for(dbname in names(xrefs)) {
        let idlist = xrefs[[dbname]];
        let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

        if (length(idlist) > 0) {
            for(id in idlist) {
                if (nchar(id) > 0) {
                    if (!(db_xrefs |> check(obj_id = mol$id,
                        db_key = db_key,
                        xref = id,
                        type = term_metabolite ))) {
                            db_xrefs |> add(
                                obj_id = mol$id,
                                db_key = db_key,
                                xref = id,
                                type =term_metabolite
                            );
                        }
                }
            }
        }
    }

    let synonyms = biocad_registry |> table("synonym");

    for(name in compound$synonym) {
        let hash = md5(tolower(name));
        let check = synonyms |> check(obj_id = mol$id, type_id = term_metabolite, hashcode = hash);

        if (!check) {
            synonyms |> add(
                obj_id = mol$id,
                type_id = term_metabolite,
                hashcode = hash,
                synonym = name,
                lang = 'en'
            );
        }        
    }
}