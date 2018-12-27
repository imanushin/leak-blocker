ECHO ***** Transforming %2 *****

SET OLDDIR1=%CD%
CD %1

SET SOLUTIONDIR=%CD%

CD %~dp2

attrib -R "%~dp2%~n2.*"
"%~dp0TextTransform.exe" %2 -a "!!SolutionDir!%SOLUTIONDIR%"

CD %OLDDIR1%