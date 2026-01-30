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
