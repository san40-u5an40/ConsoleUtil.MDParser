# ContentsMdGenerator
## Назначение:
Эта программа позволяет в одну небольшую команду составлять оглавление для указанных *.md файлов. 

## Примеры использования:
Для примера возьмём папку с README.md:
```CMD
Z:\4_programming\csharp\projects\ExtraLib\ExtraLib>dir

 Содержимое папки Z:\4_programming\csharp\projects\ExtraLib\ExtraLib

13.10.2025  23:08    <DIR>          .
13.10.2025  23:08    <DIR>          ..
13.10.2025  13:55    <DIR>          bin
12.10.2025  22:30             2 322 Bytes.cs
13.10.2025  13:10             1 862 Comparator.cs
12.10.2025  22:28             1 482 ConsoleExtension.cs
12.10.2025  22:28             5 076 Display.cs
12.10.2025  22:27               276 ExtraLib.csproj
13.10.2025  22:59    <DIR>          obj
13.10.2025  14:06            21 137 README.md
12.10.2025  22:28            18 563 Reflection.cs
12.10.2025  22:28             3 786 Regexes.cs
12.10.2025  22:28             2 901 StringCrypt.cs
12.10.2025  22:28               961 TimerHelper.cs
              10 файлов         58 366 байт
```

Вызываем программу, передавая в неё название файла, оглавление которого необходимо составить, и, при необходимости, задаём лимит для уровня заголовков, которые попадут в это оглавление.
```CMD
::                                  Вызов программы ↓     ↓ Файл  ↓ Лимит для уровня заголовка           
Z:\4_programming\csharp\projects\ExtraLib\ExtraLib>cont README.md 2
Файл "contents_README.md" успешно создан!
::            ↑ Здесь находится составленный заголовок
```

Содержимое созданного файла:
```CMD
Z:\4_programming\csharp\projects\ExtraLib\ExtraLib>type contents_README.md
    - [Regexes]()
    - [Bytes]()
    - [Comparator]()
    - [TimerHelper]()
    - [ConsoleExtension]()
    - [Display]()
    - [StringCrypt]()
    - [Reflection]()

Z:\4_programming\csharp\projects\ExtraLib\ExtraLib>
```

## Перспективы развития
В разработке возможность указать ссылку на GitHub, для формирования оглавления по содержимому README.md, выложенному в удалённом репозитории.