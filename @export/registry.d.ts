// export R# package module type define for javascript/typescript language
//
//    imports "registry" from "biocad_registry";
//
// ref=biocadRegistry.registry@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace registry {
   /**
     * @param env default value Is ``null``.
   */
   function imports_kegg_reactions(registry: object, kegg: any, env?: object): any;
   /**
   */
   function imports_metacyc_compounds(registry: object, metacyc: object): any;
   /**
   */
   function imports_metacyc_reactions(registry: object, metacyc: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_mona(registry: object, mona: any, env?: object): any;
   /**
     * @param skip_prefix default value Is ``0``.
     * @param env default value Is ``null``.
   */
   function imports_pubchem(registry: object, pubchem: any, skip_prefix?: object, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_rhea(registry: object, rhea: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_spectraverse(registry: object, spectraverse: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function make_genbank_dbxrefs(registry: object, genbank: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function save_genbank(registry: object, genbank: any, env?: object): any;
   /**
    * Save RegPrecise regulation network
    * 
    * 
     * @param registry -
     * @param genomes -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function save_TRN(registry: object, genomes: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function save_uniprot(registry: object, uniprot: any, env?: object): any;
   /**
   */
   function update_logo(registry: object): any;
}
