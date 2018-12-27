#pragma once

#undef  VER_LEGALCOPYRIGHT_STR                    
#undef  VER_COMPANYNAME_STR 
#undef  VER_PRODUCTNAME_STR    
#undef  VER_FILEVERSION                           
#undef  VER_FILEVERSION_STR                       
#undef  VER_PRODUCTVERSION                    
#undef  VER_PRODUCTVERSION_STR                      
        
#define VER_LEGALCOPYRIGHT_STR      "Copyright (C) Delta Corvi LLC 2013"        

#define COMPANYNAME_SHORT         "Delta Corvi"   
#define VER_COMPANYNAME_STR         "Delta Corvi LLC"

#define VER_PRODUCTNAME_STR        "Leak Blocker"
#define VER_FILEVERSION            /*VERSION_MACRO{0},{1},{2},{3}*/1,1,2,1
#define VER_FILEVERSION_STR        /*VERSION_MACRO"{0}.{1}.{2}.{3}"*/"1.1.2.1"
#define VER_PRODUCTVERSION         VER_FILEVERSION        
#define VER_PRODUCTVERSION_STR     VER_FILEVERSION_STR        

#define FILENAME_VERSION_PART /*VERSION_MACRO"_{0}_{1}_{2}"*/"_1_1_2"

#define VERSION_SHORT        /*VERSION_MACRO"{0}.{1}.{2}"*/"1.1.2"

