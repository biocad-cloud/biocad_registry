#' Create a new taxonomic group in the registry
#' 
#' @return the unique reference id of the new taxonomic group
#'   in the registry.
#' 
const put.taxonomic_group = function(name, id, note = "") {
    const base = getOption("biocad");
    const url  = `${base}/put/taxonomic/`;
    const resp = url 
    |> requests.post(list(name, note, id)) 
    |> http::content(throw.http.error = FALSE)
    ;

    # view of the taxonomic object put 
    # response
    str(resp);

    return(resp$info$id);
}

#' put a group of genome entry onto registry
#' 
#' @param grp the taxonomic_group id
#' @param genomes a set of genome entry info, a list set of tuple data list
#'    in format of ``list(id, name)``.
#' 
const put.genome_group = function(grp, genomes) {
    const base = getOption("biocad");
    const url  = `${base}/put/genomes/?grp=${grp}`;
    const resp = url 
    |> requests.post(list(li = genomes)) 
    |> http::content(throw.http.error = FALSE)
    ;

    str(resp);
}

const put.operon = function(genome_id, genes) {
    const base = getOption("biocad");
    const url  = `${base}/put/operons/?genome=${genome_id}`;
    const resp = url 
    |> requests.post(list(li = genes)) 
    |> http::content(throw.http.error = FALSE)
    ;

    str(resp);
}

const put.regulation = function(genome_id, regulator, family, type, sites) {
    const base = getOption("biocad");
    const url  = `${base}/put/regulation/?genome=${genome_id}&regulator=${regulator$name}&regulator_name=${urlencode(regulator$text)}`;
    const resp = url 
    |> requests.post(list(
        li = sites, 
        family = family, 
        type = as.character(type)
    ))
    |> http::content(throw.http.error = FALSE)
    ;

    str(resp);
}

const put.sequence = function(gene_id, locus_tag, gene_seq, prot_seq, note = NULL) {
    const base = getOption("biocad");
    const url = `${base}/put/seqs/?gene_id=${gene_id}&locus_tag=${locus_tag}`;
    const resp = url 
    |> requests.post(list(
        gene_seq, prot_seq, note
    ))
    |> http::content(throw.http.error = FALSE)
    ;

    str(resp);
}