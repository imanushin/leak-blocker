@ECHO ON

REM %1 - Solution folder

SET OLDDIR=%CD%
CD %1

SET SCRIPTPATH=%~dp0TransformTextTemplate.cmd
SET FOLDER=%CD%

forfiles /s /m *.tt /c "cmd /c call \"%SCRIPTPATH%\" \"%FOLDER%\" @path"

CD %OLDDIR%
