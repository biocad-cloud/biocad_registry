#' imports enzymatic kinetics model
#' 
#' @param repo a character vector of the xml data files for the enzymatic kinetics data
#' 
const imports_sabiork = function(biocad_registry, repo) {
    imports "sabiork" from "biosystem";

    for(let filepath in tqdm(repo)) {
        let sbml = parseSbml(filepath |> readText());

        if ([sbml]::empty) {
            next;
        } else {
            sbml <- unset_sbml(sbml);
        }

        print(sbml);
        stop();
    }
}