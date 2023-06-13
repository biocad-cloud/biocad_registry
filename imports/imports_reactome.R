require(GCModeller);
require(graphQL);
require(biocad_registry);

imports "SBML" from "biosystem";
imports "mysql" from "graphR";
imports "models" from "biocad_registry";

const repo = "E:\UniProt\all_species.3.1.sbml";
const models = list.files(repo, pattern = "*.sbml");
const mysql = mysql::create_filedump(dir = `${@dir}/reactions/`);

print(basename(models));

for(file in models) {
    let model = read.sbml(file);
    let compartments = extract.compartments(model, json = TRUE);
    let compounds = extract_compounds(model, json = TRUE);
    let reactions = extract_reactions(model, json = TRUE);
    let pwy_model = extract.pathway_model(model);

    # compounds = compounds[order(compounds$name), ];

    str(compartments);
    str(compounds);
    str(reactions);

    mysql |> write_dumps(compounds |> which(c -> length(c$components) > 0) |> sapply(function(c) {
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

    mysql |> write_dumps(new pathway(
        id = pwy_model$id,
        name = pwy_model$name,
        add_time = now(),
        source = 1,
        note = pwy_model$notes
    ));

    mysql |> write_dumps(sapply(compounds, function(c) {
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

    mysql |> write_dumps(sapply(compounds, function(c) {
        sapply(c$is, function(cid) {
            new dblinks(
                db_src = 1,
                xref_id = cid,
                entity_id = c$id,
                entity_type = 1,
               add_time = now() 
            ); 
        });
    }) |> unlist());

    mysql |> write_dumps(sapply(reactions, function(r) {
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

    mysql |> write_dumps(sapply(reactions, function(r) {
        sapply(r$is, function(ext_id) {
            new dblinks(
                db_src = 1,
                xref_id = ext_id,
                entity_id = r$id,
                entity_type = 2,
                add_time = now()
            );
        });
    }) |> unlist());

    mysql |> write_dumps(sapply(reactions, function(r) {
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

    mysql |> write_dumps(sapply(compartments, function(c) {
        new subcellular_compartments(
            id = c$id,
            name = c$name,
            add_time = now(),
        );
    }));

    mysql |> write_dumps(sapply(compartments, function(c) {
        sapply(c$is, function(cid) {
            new dblinks(
                db_src = 1,
                xref_id = cid,
                entity_id = c$id,
                entity_type = 4,
                add_time = now()
            );
        });
    }) |> unlist());

    mysql |> write_dumps(sapply(reactions, function(r) {
        new subcellular_locations(
            biological_process = r$id,
            compartment = as.integer($"\d+"(r$compartment))
        );
    }));

    # stop();
    invisible(NULL);
}

print(pathway);

close(mysql);