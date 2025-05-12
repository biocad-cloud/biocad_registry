require(biocad_registry);

imports "massbank" from "mzkit";
imports "data_imports" from "biocad_registry";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let mona = read.MoNA([
    "F:\datapool\mona\MoNA-export-Fiehn_HILIC.msp"
    "F:\datapool\mona\MoNA-export-Fungicide_suspect_metabolites_NCU_HIL_POS.msp"
    "F:\datapool\mona\MoNA-export-Fungicide_suspect_metabolites_NCU_RP_NEG.msp"
    "F:\datapool\mona\MoNA-export-GNPS.msp"
    "F:\datapool\mona\MoNA-export-HCD_natural_product_library.msp"
    "F:\datapool\mona\MoNA-export-HMDB.msp"
    "F:\datapool\mona\MoNA-export-Human_Plasma_Quant.msp"
    "F:\datapool\mona\MoNA-export-LC-MS_Spectra.msp"
    "F:\datapool\mona\MoNA-export-LipidBlast.msp"
    "F:\datapool\mona\MoNA-export-LipidBlast_2022.msp"
    "F:\datapool\mona\MoNA-export-MassBank.msp"
    "F:\datapool\mona\MoNA-export-PFP_NP_library.msp"
    "F:\datapool\mona\MoNA-export-QiaoLab_PGN.msp"
    "F:\datapool\mona\MoNA-export-ReSpect.msp"
    "F:\datapool\mona\MoNA-export-RIKEN_IMS_Oxidized_Phospholipids.msp"
    "F:\datapool\mona\MoNA-export-RTX5_Fiehnlib.msp"
    "F:\datapool\mona\MoNA-export-UVPD_Library.msp"
    "F:\datapool\mona\MoNA-export-Vaniya-Fiehn_Natural_Products_Library.msp"
    "F:\datapool\mona\MoNA-export-VF-NPL_LTQ.msp"
    "F:\datapool\mona\MoNA-export-VF-NPL_QExactive.msp"
    "F:\datapool\mona\MoNA-export-VF-NPL_QTOF.msp"
    "F:\datapool\mona\MoNA-export-Alkaloids_QE_pos.msp"
    "F:\datapool\mona\MoNA-export-CD_annotated_EnvCpd_suspect_metabolites_NCU_HILIC_NEG.msp"
    "F:\datapool\mona\MoNA-export-EMBL-MCF_2.0_HRMS_Library.msp"
    "F:\datapool\mona\MoNA-export-FAHFA.msp"
], lazy = FALSE);

biocad_registry |> imports_metab_repo(mona, lazy_molecule_ctor = FALSE);