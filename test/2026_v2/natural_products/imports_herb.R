require(biocad_registry);

imports "registry" from "biocad_registry";
imports ["massbank", "annotation"] from "mzkit";

let data = read.table("D:\datapool\natural_products\herbs\HERB_ingredient_info_v2.txt", row.names = NULL, header = TRUE, check.names = FALSE);

data[,"Isomeric_smiles"] = NULL;
data[,c("InChI","InChIKey","MolWt","NumHAcceptors","NumHDonors","MolLogP","NumRotatableBonds","Drug_likeness","OB_score")] = NULL;
data[,c("SymMap_id","TCMID_id","TCM_ID_id","HIT_id")] = NULL;
data[,"TCMSP_id"] = NULL;
data[,"NPASS_id"] = NULL;

print(head(data));

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let metabo = as.list(data,byrow = TRUE)
    |> tqdm()
    |> lapply(function(m) {
        let smiles = m$Canonical_smiles;
        let cas_id = m$CAS_id;
        cas_id = ifelse(str_empty(cas_id,TRUE,TRUE), NULL, cas_id);
        let db_xrefs = annotation::xref(
            SMILES   = ifelse(str_empty(smiles,TRUE,TRUE),NULL, smiles),
            pubchem  = m$PubChem_id,
            KEGG     = NULL,
            MetaCyc  = NULL,
            KNApSAcK = NULL,
            HMDB     = NULL,
            DrugBank = m$DrugBank_id,
            CAS      = strsplit(cas_id,"\s*[;,]\s*")
        );

        metabo_anno(
            id = m$Ingredient_id,
            formula = m$Molecular_formula,
            name = m$Ingredient_name,
            xref = db_xrefs,
            synonym = strsplit(m$Ingredient_alias_name, "\s*;\s*")
        );
    })
    ;

str(metabo);

biocad_registry |> imports_metab_repo(metab = metabo,
                        db_name = "HERB");
