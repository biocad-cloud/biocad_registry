require(GCModeller);
require(graphQL);
require(biocad_registry);

imports "SBML" from "biosystem";
imports "mysql" from "graphR";
imports "models" from "biocad_registry";

const repo = "E:\UniProt\all_species.3.1.sbml";
const models = list.files(repo, pattern = "*.sbml");

let reaction_node = [];
let subcellular_compartments = [];
let subcellular_locations = [];

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
            n_regulator = length(r$modifiers),
            note = r$notes
        );
    }));
    subcellular_compartments = append(subcellular_compartments, sapply(compartments, function(c) {
        new subcellular_compartments(
            id = c$id,
            name = c$name,
            add_time = now(),
        );
    }));

    subcellular_locations = append(subcellular_locations, sapply(reactions, function(r) {
        new subcellular_locations(
            biological_process = r$id,
            compartment = as.integer($"\d+"(r$compartment))
        );
    }));

    # stop();
    invisible(NULL);
}

reaction_node
|> append(subcellular_compartments)
|> append(subcellular_locations)
|> mysql::dump_inserts(dir = `${@dir}/reactions/`)
;