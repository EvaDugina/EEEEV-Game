# EEEV Game

## Настройка

1. Не забыть дописать данные в .git/config для UnityYamlMerge:
````
[merge]
	tool = unityyamlmerge
[mergetool "unityyamlmerge"]
	trustExitCode = false
 	cmd = '{путь до Unity}/Editor/Data/Tools/UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
````

### Ссылки
1. UNITY + GIT:
- https://habr.com/ru/articles/493488/
- https://unityatscale.com/unity-version-control-guide/how-to-setup-unity-project-on-github/
- https://docs.unity3d.com/Manual/SmartMerge.html
3. VISUAL STUDIO + GIT
- https://www.youtube.com/watch?v=8zSVvTQXSIc
