imports "GenBank" from "seqtoolkit";

#' imports gene from ncbi genbank file
#' 
#' @param genebank A ncbi genebank data object, all CDS/rna features inside 
#'    this genebank data object will be extract and imports into
#'    database.
#' 
#' @details this function will make association between the gene/rna 
#'    and uniprot protein molecules.
#' 
const imports_genebank = function(biocad_registry, genebank) {
    let term_gene = biocad_registry |> gene_term();
    let genes = genebank |> enumerateFeatures(keys = ["CDS","tRNA","ncRNA","rRNA"]);
    let gene_pool =  biocad_registry |> table("molecule");
    let sgt = SGT(alphabets = bioseq.fasta::chars("DNA"));
    let db_xrefs = biocad_registry |> table("db_xrefs");
    let Nucleotide_graph = biocad_registry |> vocabulary_id("Nucleotide_graph","Embedding", 
        desc =bencode( [sgt]::feature_names)
    );    

    for(gene in tqdm(genes)) {
        let nt_seq = as.fasta(gene);

        gene <- featureMeta(gene );

        let note_str = gene$product;
        let locus_tag = gene$locus_tag;
        let gene_synonym = strsplit(gene$gene_synonym, ";\s*"); 
        let db_xref =   tagvalue(gene$db_xref ,
                delimiter = ":",
                trim_value = TRUE,
                as_list = TRUE,
                union_list = TRUE);

        gene <- gene$gene;

        let mol = gene_pool   |> where(type = term_gene ,
            xref_id =locus_tag) |> find();

        if (is.null(mol)) {
            gene_pool |> add(
                xref_id = locus_tag,
                name = gene,
                mass = bioseq.fasta::mass(nt_seq, type="DNA"),
                type =  term_gene,
                formula = seq_formula(nt_seq, type="DNA"),
                parent = 0,
                note = note_str
            );

            mol =gene_pool  |> where(type =term_gene,
                xref_id = locus_tag ) |> find();
        }

        if (is.null(mol)) {
            # error while add new metabolite
            next;
        } else {
            biocad_registry |> save_nucleotide_embedding(
                mol_id = mol$id,
                dnaseq = [nt_seq]::SequenceData,
                sgt = sgt,
                Nucleotide_graph = Nucleotide_graph
            );
        }      

        for(dbname in names(db_xref)) {
            let idlist = db_xref[[dbname]];
            let db_key = biocad_registry |> vocabulary_id(dbname, "External Database");

            for(id in idlist) {
                if (!(db_xrefs |> check(obj_id = mol$id,
                    db_key = db_key,
                    xref = id,
                    type = term_gene ))) {
                        db_xrefs |> add(
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

const save_nucleotide_embedding = function(biocad_registry, mol_id, dnaseq, sgt, Nucleotide_graph) {
    let fa_vec = sgt |> fit_embedding(dnaseq);
    let seq_graph = biocad_registry |> table("sequence_graph");

    if (!(seq_graph |> check(molecule_id = mol_id))) {
        fa_vec <- base64(packBuffer(fa_vec)|> zlib_stream());
        seq_graph |> add(
            molecule_id = mol_id,
            sequence = dnaseq,
            seq_graph = fa_vec,
            embedding = Nucleotide_graph,
            hashcode = md5(tolower(dnaseq))
        );
    }
}