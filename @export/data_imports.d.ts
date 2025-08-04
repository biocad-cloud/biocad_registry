// export R# package module type define for javascript/typescript language
//
//    imports "data_imports" from "biocad_registry";
//
// ref=biocadRegistry.imports_api@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace data_imports {
   /**
   */
   function genbank_repo(dir: string): object;
   /**
   */
   function imports_chebi_repo(registry: object, chebi: object): ;
   /**
   */
   function imports_dbxrefs(registry: object, genbank: object): any;
   /**
   */
   function imports_genbank(registry: object, genbank: object): any;
   /**
   */
   function imports_genbank_proteins(registry: object, genbank: object): any;
   /**
   */
   function imports_genes(registry: object, genbank: object): any;
   /**
   */
   function imports_genomics(registry: object, genbank: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_lotus_np(registry: object, lotus: any, env?: object): any;
   /**
     * @param lazy_molecule_ctor default value Is ``false``.
     * @param topic default value Is ``null``.
     * @param exclude_topic default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function imports_metab_repo(registry: object, metab: any, lazy_molecule_ctor?: boolean, topic?: string, exclude_topic?: string, env?: object): any;
   /**
   */
   function imports_pubchem_repo(registry: object, repo: object): ;
   /**
   */
   function imports_refmet(registry: object, file: string): ;
   /**
   */
   function imports_taxonomy(registry: object, taxdump: object): any;
   /**
   */
   function imports_uniprot(registry: object, uniprot: string): any;
   /**
   */
   function pubchem_repo(dir: string): object;
}
