// export R# package module type define for javascript/typescript language
//
//    imports "models" from "biocad_registry";
//
// ref=biocadRegistry.registry_models@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace models {
   /**
   */
   function build_microbial_nps(registry: object): ;
   /**
   */
   function build_plantnp_library(registry: object): ;
   /**
     * @param check_unique default value Is ``false``.
     * @param batch_size default value Is ``500000``.
     * @param env default value Is ``null``.
   */
   function imports_diamond(registry: object, blastp: any, check_unique?: boolean, batch_size?: object, env?: object): any;
   /**
     * @param env default value Is ``null``.
   */
   function imports_pathways(registry: object, pathways: any, env?: object): any;
   /**
   */
   function register_metabolic_symbols(registry: object): ;
   /**
   */
   function resolve_metabolite_duplicates(registry: object): ;
   /**
    * make updates of the compartment location metadata
    * 
    * 
     * @param registry -
     * @param locations -
   */
   function update_location(registry: object, locations: object): ;
   /**
   */
   function update_metabolic_network(registry: object): ;
}
