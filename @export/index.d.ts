// export R# source type define for javascript/typescript language
//
// package_source=biocad_registry

declare namespace biocad_registry {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
   */
   function gene_term(biocad_registry: any): object;
   /**
   */
   function imports_uniprot(biocad_registry: any, uniprot: any): object;
   /**
   */
   function metabolite_term(biocad_registry: any): object;
   /**
     * @param host default value Is ``localhost``.
     * @param port default value Is ``3306``.
     * @param dbname default value Is ``cad_registry``.
   */
   function open_registry(user: any, passwd: any, host?: any, port?: any, dbname?: any): object;
   /**
   */
   function protein_term(biocad_registry: any): object;
   /**
   */
   function rna_term(biocad_registry: any): object;
}
