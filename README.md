# ContentsMdGenerator
## Назначение
Эта программа позволяет извлекать разные данные из *.md файлов. 

## Команды
- `content` — Команда для извлечения оглавления из текста.
    - `-f` `{имя файла}` `*` — Источник текста.
    - `-l` `{число от 1 до 6}` — Лимит для уровня заголовков. `По умолчанию: 6`.
    - `-a` — Наличие якорей в оглавлении. `По умолчанию: false`.
- `links` — Команда для извлечения ссылок.
    - `-f` `{имя файла}` `*` — Источник текста.
    - `-r` — Наличие источника ссылки. `По умолчанию: false`.
    - `-m` `{маска для интернирования}` — С помощью символа `@1` можно интернировать ссылку, а с помощью `@2` её источник. По умолчанию используются `- @1` для ссылок без источника и `- [@1](@2)` для ссылок с источниками.

`*` — Обязательные опции.

## Примеры использования
Команда:\
`markdown content -f README.md -l 2`

Содержимое файла:
```
- [san40_u5an40.ExtraLib]()
    - [Оглавление:]()
    - [Bytes]()
    - [Comparator]()
    - [Counter]()
    - [DefaultConstants]()
    - [EmailsParser]()
    - [HtmlHelper]()
    - [MessageBox]()
    - [StringExtension]()
    - [TimerHelper]()
    - [Readyable]()
    - [Result]()
    - [Chain and AsyncChain]()
    - [ConsoleExtension]()
    - [StringCrypt]()
    - [Reflection]()
    - [История последних изменений]()
```
---

Команда:\
`markdown content -f README.md -l 2 -a` (добавились якоря)

Содержимое файла:
```
- [san40_u5an40.ExtraLib](#san40_u5an40.extralib)
    - [Оглавление:](#оглавление:)
    - [Bytes](#bytes)
    - [Comparator](#comparator)
    - [Counter](#counter)
    - [DefaultConstants](#defaultconstants)
    - [EmailsParser](#emailsparser)
    - [HtmlHelper](#htmlhelper)
    - [MessageBox](#messagebox)
    - [StringExtension](#stringextension)
    - [TimerHelper](#timerhelper)
    - [Readyable](#readyable)
    - [Result](#result)
    - [Chain and AsyncChain](#chain-and-asyncchain)
    - [ConsoleExtension](#consoleextension)
    - [StringCrypt](#stringcrypt)
    - [Reflection](#reflection)
    - [История последних изменений](#история-последних-изменений)
```

`Важно!` Так как на разных платформах отличаются правила обработки специальных символов во внутренних ссылках, программа использует минимальный набор:
- Приводит всё к нижнему регистру.
- Заменяет пробелы на '-'.

При наличии специальных или пунктуационных символов и специфических требований платформы может понадобится ручная корректировка ссылок, указанных в оглавлении.

---

Команда:\
`markdown links -f README.md`

Содержимое файла:
```
- в конце
- тут
- разделе
```
---

Команда:\
`markdown links -f README.md -r` (добавился источник ссылок)

Содержимое файла:
```
- [в конце](#история-последних-изменений)
- [тут](https://github.com/san40-u5an40/ConsoleUtil.MDParser)
- [разделе](#result)
```
---

Команда:\
`markdown links -f README.md -m "@1 ← это ссылка, @2 ← а это её источник" -r` (добавилась маска для интернирования)

Содержимое файла:
```
в конце ← это ссылка, #история-последних-изменений ← а это её источник
тут ← это ссылка, https://github.com/san40-u5an40/ConsoleUtil.ContentsMdGenerator ← а это её источник
разделе ← это ссылка, #result ← а это её источник
```