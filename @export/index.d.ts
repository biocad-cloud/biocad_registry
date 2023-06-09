// export R# source type define for javascript/typescript language
//
// package_source=biocad_registry

declare namespace biocad_registry {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   gene_dblinks: string;
   /**
     * @param page default value Is ``1``.
     * @param page_size default value Is ``1000``.
   */
   function get_dblinks(dbname: any, page?: object, page_size?: object): object;
   /**
     * @param fasta default value Is ``false``.
   */
   function get_motif_sites(family: any, fasta?: boolean): object;
   go_namespace: any;
   motif_sites_family: string;
   /**
     * @param genome_id default value Is ``null``.
   */
   function pull_motif_family(genome_id?: any): object;
   /**
     * @param id default value Is ``null``.
   */
   function pull_taxonomic(id?: any): object;
   module put {
      /**
      */
      function genome_group(grp: any, genomes: any): object;
      /**
      */
      function go_term(term: any): object;
      /**
      */
      function ncbi_tax(tax: any): object;
      /**
      */
      function operon(genome_id: any, genes: any): object;
      /**
      */
      function regulation(genome_id: any, regulator: any, family: any, type: any, sites: any): object;
      /**
        * @param note default value Is ``null``.
      */
      function sequence(gene_id: any, locus_tag: any, gene_seq: any, prot_seq: any, note?: any): object;
      /**
        * @param note default value Is ````.
      */
      function taxonomic_group(name: any, id: any, note?: string): object;
   }
   /**
   */
   function wrap_list(a: any): object;
}
