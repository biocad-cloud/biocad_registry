require(GCModeller);

imports "SBML" from "biosystem";

const repo = "E:\UniProt\all_species.3.1.sbml";
const models = list.files(repo, pattern = "*.sbml");

print(basename(models));

for(file in models) {
    let model = read.sbml(file);
    let compartments = extract.compartments(model);
    let compounds = extract_compounds(model);

    compounds = compounds[order(compounds$name), ];

    print(compartments);
    print(compounds);

    stop();
}