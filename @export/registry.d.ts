// export R# package module type define for javascript/typescript language
//
//    imports "registry" from "biocad_registry";
//
// ref=biocadRegistry.registry@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace registry {
   /**
     * @param env default value Is ``null``.
   */
   function make_genbank_dbxrefs(registry: object, genbank: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function save_genbank(registry: object, genbank: any, env?: object): any;
}
