# シナリオの書き方
以下のように、ラベル・コマンド・テキストを書きます。
```
* ラベル名
[コマンド名 変数=値]
テキスト
```
```
*Start
[clear]
[actor name=Robo texture=robo]
やあ、こんにちは！[p]
```
- ラベル：分岐の移動先やセーブ地点として使用します。
- コマンド：キャラの表示・クリック待ち・エフェクト再生などを実行します。
- テキスト：文章を表示します。

# ファイル
## 本番用のファイル
`Assets/StreamingAssets`に配置します。
## 作業用のファイル
[Application.persistentDataPath](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)に配置します。ゲーム実行時、これらのファイルは本番用ファイルより優先して読み込まれます。

# 目次
- [コマンド](Command.md)
- [コマンド一覧](CommandList.md)
