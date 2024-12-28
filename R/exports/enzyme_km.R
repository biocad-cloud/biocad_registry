const export_enzymatic = function(biocad_registry) {
    let sql = "
        SELECT 
            DISTINCT  
            params->>'$.Km' as Km,temperature,pH,substrate_id, uniprot,ec_number,seq_graph
        FROM
            cad_registry.kinetic_law
                LEFT JOIN
            db_xrefs ON db_key = 40 AND db_xrefs.type = 3
                AND xref = uniprot
                LEFT JOIN 
            cad_registry.sequence_graph ON cad_registry.sequence_graph.molecule_id = obj_id
        WHERE
            (NOT xref IS NULL) 
            AND (substrate_id > 0) 
            AND (params->>'$.Km' > 0) 
            AND (not seq_graph is null)
        ;"
    ;

    biocad_registry 
    |> table("kinetic_law") 
    |> exec(sql)
    ;
} 