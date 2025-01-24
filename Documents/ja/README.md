# シナリオの書き方
シナリオは文章・コマンド・ラベルを組み合わせて書きます。
- 文章：セリフなどの文章です。
- \[コマンド\]：プログラムに命令します。背景やキャラの表示などをします。
- *ラベル：章の始まりの印です。分岐をするときに指定します。
```
*Start
[clear]
[actor name=Robo texture=robo]
やあ、こんにちは！
```

`;`でコメントを書けます。
```
文章です ;コメントです
```

# ファイルの場所
## 本番用のファイル
`Assets/StreamingAssets`に配置します。
## 作業用のファイル
[Application.persistentDataPath](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)に配置します。ゲーム実行時、これらのファイルは本番用ファイルより優先して読み込まれます。

# 目次
- [コマンドについて](Command.md)
- [コマンド一覧](CommandList.md)
