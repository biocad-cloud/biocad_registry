// export R# package module type define for javascript/typescript language
//
//    imports "registry" from "biocad_registry";
//
// ref=biocad_registry.registry@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace registry {
   /**
     * @param direct_list default value Is ``true``.
   */
   function child_list(registry: object, tax_id: string, direct_list?: boolean): any;
   /**
   */
   function find_taxinfo(registry: object, tax: string): object;
   /**
   */
   function get_taxinfo(registry: object, tax: string): object;
   /**
   */
   function taxonomy_lineage(registry: object, tax_id: string): object;
}
