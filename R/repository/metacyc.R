imports "BioCyc" from "annotationKit";

#' helper function for imports the metacyc molecules and reactions
#' 
const imports_metacyc = function(biocad_registry, metacyc) {
    metacyc <- open.biocyc(metacyc);

    biocad_registry |> load_biocyc_proteins(metacyc);
    # biocad_registry |> load_biocyc_genes(metacyc);
    # biocad_registry |> load_biocyc_reactions(metacyc);
    # biocad_registry |> load_biocyc_compounds(metacyc);
}

const load_biocyc_proteins = function(biocad_registry, metacyc) {
    let proteins = metacyc |> BioCyc::getProteins(metacyc, protseq = file.path(metacyc, "protseq.fsa"));
    let sgt = SGT(alphabets = bioseq.fasta::chars("Protein"));
    let term_prot = biocad_registry::protein_term(biocad_registry);
    let entity_prot = biocad_registry::molecule_entity(biocad_registry);
    let db_xrefSet = biocad_registry |> table("db_xrefs");
    let protein_graph = biocad_registry |> vocabulary_id("Protein_graph","Embedding", 
        desc =bencode( [sgt]::feature_names)
    );
    let prot_pool = biocad_registry |> table("molecule");
    let seq_graph = biocad_registry |> table("sequence_graph");

    for(let aa in tqdm(proteins)) {
        let db_xrefs = BioCyc::db_links(aa);

        aa <- as.list(aa);
        db_xrefs$BioCyc <- aa$uniqueId;

        let mass = bioseq.fasta::mass(aa$protseq , type="Protein");
        let prot_ids = unlist(db_xrefs) |> unlist() |> append(aa$uniqueId);

        prot_ids <- prot_ids[nchar(prot_ids) > 0];

        if (nchar(aa$protseq) == 0) {
            aa$protseq <- "";
            mass <- 0;
        }

        let mol = prot_pool
            |> left_join("db_xrefs") 
            |> on(db_xrefs.obj_id = molecule.id)  
            |> where(molecule.type = term_prot ,
                    mass between [mass * 0.85, mass *1.25],
                    db_xrefs.xref in prot_ids) 
            |> find()
            ;

        if (is.null(mol)) {
            prot_pool |> add(
                xref_id = aa$uniqueId,
                name = aa$commonName,
                mass = mass,
                type = term_prot,
                formula = seq_formula(aa$protseq, type="Protein"),
                parent = 0,
                note = (aa$comment) || (aa$commonName)
            );

            mol = prot_pool |> where(type = term_prot,
                xref_id = aa$uniqueId) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            for(dbname in names(db_xrefs)) {
                let idlist = db_xrefs[[dbname]];
                let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

                for(id in idlist) {
                    if (!(db_xrefSet |> check(obj_id = mol$id,
                        db_key = db_key,
                        xref = id,
                        type = term_prot ))) {
                            db_xrefSet |> add(
                                obj_id = mol$id,
                                db_key = db_key,
                                xref = id,
                                type = term_prot 
                            );
                        }
                }
            }     

            if (!(seq_graph |> check(molecule_id = mol$id))) {
                let fa_vec = sgt |> fit_embedding(aa$protseq,safe =TRUE);

                fa_vec <- base64(packBuffer(fa_vec)|> zlib_stream());

                seq_graph |> add(
                    molecule_id = mol$id,
                    sequence = aa$protseq,
                    seq_graph = fa_vec,
                    embedding = protein_graph,
                    hashcode = md5(tolower(aa$protseq))
                );
            }
        }
    }
}

const load_biocyc_genes = function(biocad_registry, metacyc) {
    let genes = metacyc |> BioCyc::getGenes(metacyc, dnaseq = file.path(metacyc, "dnaseq.fsa"));
    let term_gene = biocad_registry |> gene_term();
    let gene_pool =  biocad_registry |> table("molecule");
    let sgt = SGT(alphabets = bioseq.fasta::chars("DNA"));
    let db_xrefSet = biocad_registry |> table("db_xrefs");
    let Nucleotide_graph = biocad_registry |> vocabulary_id("Nucleotide_graph","Embedding", 
        desc =bencode( [sgt]::feature_names)
    );    

    for(let gene in tqdm(genes)) {
        let db_xrefs = BioCyc::db_links(gene);

        gene <- as.list(gene);
        db_xrefs$BioCyc <- gene$uniqueId;

        let mass = bioseq.fasta::mass(gene$dnaseq, type="DNA");
        let gene_ids = [gene$accession1, gene$accession2, gene$uniqueId] |> append(unlist(unlist(db_xrefs)));

        gene_ids <- gene_ids[nchar(gene_ids) > 0];

        if (nchar(gene$dnaseq) == 0) {
            gene$dnaseq = "";
            mass = 0;
        }

        let mol = gene_pool 
            |> left_join("db_xrefs") 
            |> on(db_xrefs.obj_id = molecule.id)  
            |> where(molecule.type = term_gene ,
                    mass between [mass * 0.85, mass *1.25],
                    db_xrefs.xref in gene_ids) 
            |> find()
            ;

        if (is.null(mol)) {
            gene_pool |> add(
                xref_id = gene$uniqueId,
                name = gene$commonName,
                mass = mass,
                type =  term_gene,
                formula = seq_formula(gene$dnaseq, type="DNA"),
                parent = 0,
                note = gene$comment
            );

            mol =gene_pool  |> where(type =term_gene,
                xref_id = gene$uniqueId ) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> save_nucleotide_embedding(
                mol_id = mol$id,
                dnaseq = gene$dnaseq,
                sgt = sgt,
                Nucleotide_graph = Nucleotide_graph
            );
        }   

        for(dbname in names(db_xrefs)) {
            let idlist = db_xrefs[[dbname]];
            let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

            for(id in idlist) {
                if (!(db_xrefSet |> check(obj_id = mol$id,
                    db_key = db_key,
                    xref = id,
                    type = term_gene ))) {
                        db_xrefSet |> add(
                            obj_id = mol$id,
                            db_key = db_key,
                            xref = id,
                            type = term_gene
                        );
                    }
            }
        }
    }
}

const load_biocyc_reactions = function(biocad_registry, metacyc) {
    for(let rxn in tqdm(metacyc |> getReactions())) {
        let equation_string = rxn |> BioCyc::formula(rxn);
        let ec = [rxn]::ec_number;
        let left = lapply([rxn]::left, function(c) {
            list(side = "substrate", compound = list(
                entry = [c]::ID ,
                name = [c]::ID,
                formula = [c]::ID, 
                factor = as.numeric([c]::Stoichiometry)
            ));
        });
        let right = lapply( [rxn]::right, function(c) {
            list(side = "product", compound = list(
                entry = [c]::ID ,
                name = [c]::ID,
                formula = [c]::ID, 
                factor = as.numeric([c]::Stoichiometry)
            ));
        });

        rxn <- as.list(rxn);
        rxn <- list(
            entry = rxn$uniqueId,
            definition = equation_string,
            comment = rxn$comment,
            enzyme = [ec]::ECNumberString,
            db_xrefs = [],
            compounds = append(left,right)
        );
        
        biocad_registry |> push_reaction(reaction = rxn);
    }
}

#' load and imports compounds from the metacyc database
#'
const load_biocyc_compounds = function(biocad_registry, metacyc) {
    let term_metabolite   = biocad_registry::metabolite_term(biocad_registry);
    let entity_metabolite = biocad_registry::molecule_entity(biocad_registry);
    # imports metabolites
    let compartments = biocad_registry |> table("subcellular_compartments");
    let location_link = biocad_registry |> table("subcellular_location");
    let metabolite = biocad_registry |> table("molecule");    

    for(let meta in tqdm(metacyc |> getCompounds())) {
        let formula_str = meta |> BioCyc::formula(meta);
        let dbkeys = meta |> BioCyc::db_links(meta);

        meta <- as.list(meta);
        meta$formula <- formula_str;
        meta <- list(
            ID = meta$uniqueId,
            formula = formula_str,
            exact_mass = 0,
            name = ifelse(nchar(meta$commonName) > 0, meta$commonName, meta$uniqueId),
            IUPACName = meta$commonName,
            description = meta$comment,
            synonym = meta$synonyms,
            xref = list(
                chebi = {
                    if (length(dbkeys$chebi) > 0) {
                        `CHEBI:${dbkeys$chebi}`;
                    } else {
                        NULL;
                    }
                },
                KEGG = dbkeys$kegg,
                pubchem = dbkeys$pubchem,
                HMDB = dbkeys$hmdb,
                Wikipedia = dbkeys$wikipedia,
                lipidmaps = dbkeys$lipidmaps,
                DrugBank = dbkeys$drugbank,
                MeSH = dbkeys$mesh,
                MetaCyc = meta$uniqueId,
                foodb = dbkeys$foodb,
                CAS = dbkeys$cas,
                InChIkey = (meta$InChIKey) || "-",
                InChI = (meta$InChI) || (meta$nonStandardInChI),
                SMILES = meta$SMILES,
                METANETX = dbkeys$metanetx,
                refmet = dbkeys$refmet,
                Metabolights = dbkeys$metabolights,
                Bigg = dbkeys$bigg
            )
        );

        let mol = biocad_registry |> find_molecule(meta, meta$ID);

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> __push_compound_metadata(meta, mol);
        }
    }
}