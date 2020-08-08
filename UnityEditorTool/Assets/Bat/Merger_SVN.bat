@echo off

@set svn_bin=C:\Program Files\TortoiseSVN\bin
@set svn_work=D:\WorkCopy\branch2
@set trunk_path=svn://192.168.0.50/dragonunity/trunk
@set mege_range=18556-18560,18562-18565,18567-18569,18571-18580,18583,18585-18587,18589-18596,18598-18599,18604-18606,18608-18610,18612-18616,18618-18628,18633-18645,18647-18671,18674-18678,18680,18682-18683,18685,18690
@set svn_version_split="-"
@set svn_log=Merged revision(s) from trunk : %mege_range%
:: 打印
@echo   配置信息
@echo ************************************************************************
@echo  URL to merge from ：%trunk_path%
@echo  Working Copy      ：%svn_work%
@echo ------------------------------------------------------------------------
@echo  specific range    ：%mege_range%
@echo ************************************************************************


 rem 判断可执行文件及项目文件目录是否正确
@if not exist "%svn_bin%\TortoiseProc.exe" (
    echo.
    echo error: 请确认TortoiseSVN客户端目录正确?
    echo 目前定义的是:%svn_bin% 
    echo.
    pause & exit 1
    )

@if not exist "%svn_work%\.svn" (
    echo.
    echo error: 请确认SVN项目文件目录正确?
    echo 目前定义的是:%svn_work% 
    echo.
    pause & exit 2
    )

if defined mege_range (
    echo --------start--------
) else (
	echo 合并区间为空 ！
	pause & exit 2
)

::echo SVN目录更新
echo 更新svn...
"%svn_bin%"\TortoiseProc.exe/command:update /path:"%svn_work%" /closeonend:2
echo 更新完成 !

rem 遍历版本
for %%a in (%mege_range%) do (
	echo;%%a|find %svn_version_split% >nul &&(
	rem 存在“-” 区间逐个合并
		for /f "delims=-, tokens=1,2" %%i in ("%%a") do (
			for /l %%b in (%%i,1,%%j) do (
				rem echo %%b
				call:TestMerge %%b
				call:CheckTestMergeResult %%b
			)
		)
	)||(
		rem 单独版本
		rem echo %%a *
		call:TestMerge %%a
		call:CheckTestMergeResult %%a
	)
)
echo. ==================== Success!! ========
echo. ==================== Success!! ========
echo. ==================== Success!! ========
if exist MergeInfo.txt del MergeInfo.txt

echo %svn_log% | clip

:END
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
		::无冲突 可以merger
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

