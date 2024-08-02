require(biocad_registry);

imports "uniprot" from "seqtoolkit";
imports "dataset" from "MLkit";
imports "bioseq.fasta" from "seqtoolkit";

let biocad_registry = open_registry("root", 123456);
let uniprot = parseUniProt(readText(file.path(@dir,"uniprotkb_taxonomy_id_2_AND_model_organ_2024_08_02.xml")));
let sgt = SGT(alphabets = bioseq.fasta::chars("Protein"));

for(let prot in tqdm(uniprot)) {
    let fa = uniprot::get_sequence(prot);
    let info = uniprot::get_description(prot);
    let loc = uniprot::get_subcellularlocation(prot);
    let xrefs = uniprot::get_xrefs(prot);

    fa = sgt |> fit_embedding([fa]::SequenceData);
    info = paste(info, sep = "; ");

    print(fa);
    print(info);
    print(loc);
    str(xrefs);

    stop();
}