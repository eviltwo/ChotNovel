# ChotNovel
Simple novel engine for Unity.

Unity向けのシンプルなノベルゲームシステムです。

# Feature
- The script structure is based on KAG (KiriKiri) and Naninovel.
- The script is designed so that the commands and parameters written by the script creator are kept to a minimum.
- The script will be in txt format and stored in streamingAssets.
- Since the script stored in persistentDataPath is prioritized, the script writer can verify playback using only the executable file without Unity Editor.
- Commands for messages, characters, backgrounds are provided by default. (Unimplemented) Music, Sound
- Game developers can extend commands by creating components.
- (Unimplemented) Supports Unity.Localization.

# 特徴
- スクリプトの構造はKAG(吉里吉里)やNaninovelを参考にしています。
- スクリプト作成者がスクリプトに記述するコマンドやパラメータはできるだけ少なくなるように設計されています。
- スクリプトはtxt形式で、streamingAssetsに格納します。
- persistentDataPathに格納されたスクリプトが優先されるので、スクリプト作成者はUnityエディタを開かずに実行ファイルだけで再生確認ができます。
- メッセージ・キャラ・背景のコマンドはデフォルトで用意されています。(未実装)BGM・SE
- ゲーム開発者は、コンポーネントを作成することでコマンドを拡張できます。
- (未実装)Unity.Localizationに対応しています。

# Install with UPM
```
https://github.com/eviltwo/ChotNovel.git?path=ChotNovel/Assets/ChotNovel
```

# Example
https://github.com/eviltwo/ChotNovel/blob/24799f3540a574c5eea4823f5d41300826b931d5/ChotNovel/Assets/StreamingAssets/scenario/test.txt#L1-L37
