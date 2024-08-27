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
   function __push_compound_metadata(biocad_registry: any, compound: any, mol: any): object;
   /**
   */
   function check_metabolite(biocad_registry: any, compound: any): object;
   /**
   */
   function enzyme_regulation(biocad_registry: any): object;
   /**
   */
   function find_molecule(biocad_registry: any, meta: any, xref_id: any): object;
   /**
   */
   function gene_term(biocad_registry: any): object;
   /**
     * @param dbname default value Is ``null``.
   */
   function get_molecule_by_dbxref(registry: any, db_xref: any, dbname?: any): object;
   /**
   */
   function imports_chebi(biocad_registry: any, chebi: any): object;
   /**
   */
   function imports_genebank(biocad_registry: any, genebank: any): object;
   /**
   */
   function imports_metacyc(): object;
   /**
   */
   function imports_pubchem(biocad_registry: any, pubchem: any): object;
   /**
   */
   function imports_rhea(biocad_registry: any, rhea: any): object;
   /**
   */
   function imports_sabiork(biocad_registry: any, repo: any): object;
   /**
   */
   function imports_uniprot(biocad_registry: any, uniprot: any): object;
   /**
   */
   function link_gene_proteins(biocad_registry: any): object;
   /**
   */
   function link_reaction_enzymes(biocad_registry: any): object;
   /**
   */
   function link_reaction_metabolites(biocad_registry: any): object;
   /**
   */
   function metabolite_term(biocad_registry: any): object;
   /**
   */
   function molecule_entity(biocad_registry: any): object;
   /**
   */
   function molecule_terms(biocad_registry: any): object;
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
   function reaction_model(biocad_registry: any): object;
   /**
   */
   function rna_term(biocad_registry: any): object;
}
