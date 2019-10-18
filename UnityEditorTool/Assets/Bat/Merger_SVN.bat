@echo off

@set svn_bin=C:\Program Files\TortoiseSVN\bin
@set svn_work=D:\WorkCopy\branch
@set trunk_path=svn://192.168.0.50/dragonunity/trunk
@set mege_range=14762-14763,14776-14777
@set svn_version_split="-"
:: ��ӡ
@echo   ������Ϣ
@echo ************************************************************************
@echo  URL to merge from ��%trunk_path%
@echo  Working Copy      ��%svn_work%
@echo ------------------------------------------------------------------------
@echo  specific range    ��%mege_range%
@echo ************************************************************************


 rem �жϿ�ִ���ļ�����Ŀ�ļ�Ŀ¼�Ƿ���ȷ
@if not exist "%svn_bin%\TortoiseProc.exe" (
    echo.
    echo error: ��ȷ��TortoiseSVN�ͻ���Ŀ¼��ȷ?
    echo Ŀǰ�������:%svn_bin% 
    echo.
    pause & exit 1
    )

@if not exist "%svn_work%\.svn" (
    echo.
    echo error: ��ȷ��SVN��Ŀ�ļ�Ŀ¼��ȷ?
    echo Ŀǰ�������:%svn_work% 
    echo.
    pause & exit 2
    )

if defined mege_range (
    echo --------start--------
) else (
	echo �ϲ�����Ϊ�� ��
	pause & exit 2
)

::echo SVNĿ¼����
echo ����svn...
"%svn_bin%"\TortoiseProc.exe/command:update /path:"%svn_work%" /closeonend:2
echo ������� !

rem �����汾
for %%a in (%mege_range%) do (
	echo;%%a|find %svn_version_split% >nul &&(
	rem ���ڡ�-�� ��������ϲ�
		for /f "delims=-, tokens=1,2" %%i in ("%%a") do (
			for /l %%b in (%%i,1,%%j) do (
				rem echo %%a
				call:TestMerge %%b
				call:CheckTestMergeResult %%b
			)
		)
	)||(
		rem �����汾
		rem echo %%a
		call:TestMerge %%a
		call:CheckTestMergeResult %%a
	)
)
echo. ==================== Success!! ========
echo. ==================== Success!! ========
echo. ==================== Success!! ========
GOTO END

:END
if exist MergeInfo.txt del MergeInfo.txt
pause
GOTO:EOF

:TestMerge
echo. ==================== test merge %1 ... ========
svn merge %trunk_path% -c %1 --dry-run > MergeInfo.txt
GOTO:EOF

:CheckTestMergeResult
findstr .* MergeInfo.txt >nul
if errorlevel 1 (
	echo    result = false  reason : Is Empty Version OR Already Merge
) else (
	findstr "Tree conflicts" MergeInfo.txt >nul
	if errorlevel 1 (
		::�޳�ͻ ����merger
		echo    result = true  do merger...
		call:DoMerge %%1
	) else (
		echo    result = false  reason : Tree conflicts!!
		GOTO END
	)
)
GOTO:EOF

:DoMerge  
echo. ==================== do merge %1 ... ========
svn merge %trunk_path% -c %1
GOTO:EOF

