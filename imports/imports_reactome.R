require(GCModeller);
require(graphQL);
require(biocad_registry);

imports "SBML" from "biosystem";
imports "mysql" from "graphR";
imports "models" from "biocad_registry";

const repo = "E:\UniProt\all_species.3.1.sbml";
const models = list.files(repo, pattern = "*.sbml");

let reaction_node = [];

print(basename(models));

for(file in models[1:3]) {
    let model = read.sbml(file);
    let compartments = extract.compartments(model, json = TRUE);
    let compounds = extract_compounds(model, json = TRUE);
    let reactions = extract_reactions(model, json = TRUE);

    # compounds = compounds[order(compounds$name), ];

    str(compartments);
    str(compounds);
    str(reactions);

    reaction_node = append(reaction_node, sapply(reactions, function(r) {
        new reaction_node(
            id = r$id,
            name = r$name,
            add_time = now(),
            n_left = length(r$reactants),
            n_right = length(r$products),
            n_regulator = length(r$modifiers)
        );
    }));

    # stop();
    invisible(NULL);
}

mysql::dump_inserts(reaction_node, dir = `${@dir}/reactions/`);