require(biocad_registry);

imports ["massbank", "annotation"] from "mzkit";
imports "data_imports" from "biocad_registry";

let data = read.csv("D:\datapool\plants\planthub\metabolite_id.csv", row.names = NULL);
let metabo = as.list(data,byrow = TRUE)
    |> tqdm()
    |> lapply(function(m) {
        let db_xrefs = annotation::xref(
            SMILES   = m$smiles,
            pubchem  = m$pubchem,
            KEGG     = m$kegg,
            MetaCyc  = m$metacyc,
            KNApSAcK = m$knapsack,
            HMDB     = m$hmdb,
            extras = list(
                PMhub = m$id
            )
        );

        metabo_anno(
            id = m$id,
            formula = m$formula,
            name = m$name,
            xref = db_xrefs
        );
    })
    ;
let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

str(metabo);

biocad_registry |> imports_metab_repo(metab = metabo,
                        topic = "plant");
