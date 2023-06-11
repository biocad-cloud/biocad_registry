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
let reaction_graph = [];
let molecules = [];
let pathway = [];
let complex = [];

print(basename(models));

for(file in models[1:3]) {
    let model = read.sbml(file);
    let compartments = extract.compartments(model, json = TRUE);
    let compounds = extract_compounds(model, json = TRUE);
    let reactions = extract_reactions(model, json = TRUE);
    let pwy_model = extract.pathway_model(model);

    # compounds = compounds[order(compounds$name), ];

    str(compartments);
    str(compounds);
    str(reactions);

    complex = append(complex, compounds |> which(c -> length(c$components) > 0) |> sapply(function(c) {
        const n_components = length(c$components);

        # FNV1a_hashcode
        sapply(c$components, function(cid) {
            new complex(
                molecule_id =  c$id,
                component_id = FNV1a_hashcode(cid),
                n_components = n_components,
                name = c$name
            );
        });
    }) |> unlist());

    pathway = append(pathway, new pathway(
        id = pwy_model$id,
        name = pwy_model$name,
        add_time = now(),
        source = 1,
        note = pwy_model$notes
    ));

    molecules = append(molecules, sapply(compounds, function(c) {
        new molecules(
            id = c$id,
            molecule_id = c$id,
            type = 1,
            name = c$name,
            seq_num = 0,
             synonym_num = 0,
             ncbi_taxid = 0,
             category_id = 0,
             description = c$notes,
             add_time = now()
        );
    }));

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

    reaction_graph = append(reaction_graph, sapply(reactions, function(r) {
        let cpds = r$reactants;
        let rtns = sapply(names(cpds), function(id) {
            new reaction_graph(
                reaction_id = r$id,
                molecule_id = as.integer($"\d+"(id)),
                name = compounds[[id]]$name,
                stoichiometric = cpds[[id]],
                type = 1
            ); 
        });

        cpds = r$products;

        let prods = sapply(names(cpds), function(id) {
            new reaction_graph(
                reaction_id = r$id,
                molecule_id = as.integer($"\d+"(id)),
                name = compounds[[id]]$name,
                stoichiometric = cpds[[id]],
                type = 2
            ); 
        });

        let enzs = sapply(r$modifiers, function(id) {
            new reaction_graph(
                reaction_id = r$id,
                molecule_id = as.integer($"\d+"(id)),
                name = compounds[[id]]$name,
                stoichiometric = 0,
                type = 3
            ); 
        });

        append(rtns, append(prods, enzs));
    }) |> unlist());

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

print(pathway);

reaction_node
|> append(subcellular_compartments)
|> append(subcellular_locations)
|> append(reaction_graph)
|> append(molecules)
|> append(pathway)
|> append(complex)
|> mysql::dump_inserts(dir = `${@dir}/reactions/`)
;