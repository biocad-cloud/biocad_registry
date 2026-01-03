// export R# package module type define for javascript/typescript language
//
//    imports "setup" from "biocad_registry";
//
// ref=biocadRegistry.setup@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace setup {
   /**
     * @param env default value Is ``null``.
   */
   function setup_hmdb(registry: object, hmdb: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function setup_kegg(registry: object, kegg: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function setup_lipidmaps(registry: object, lipidmaps: any, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function setup_refmet(registry: object, refmet: any, env?: object): any;
}
