# MiniNovel
Unity向けの最小限のノベルゲームシステムです。
Minimal novel engine for Unity.

# Feature (Japanese)
- スクリプトの構造はKAG(吉里吉里)を参考にしています。
- スクリプトはtxt形式で、streamingAssetsに格納します。
- persistentDataPathに格納されたスクリプトが優先されるので、スクリプト作成者はUnityエディタを開かずに実行ファイルだけで再生確認ができます。
- メッセージ・キャラ・背景のコマンドはデフォルトで用意されています。(未実装)BGM・SE
- ゲーム開発者は、コンポーネントを作成することでコマンドを拡張できます。
- (未実装)Unity.Localizationに対応しています。

# Feature (English)
- The script structure is based on KAG (KiriKiri).
- The script will be in txt format and stored in streamingAssets.
- Since the script stored in persistentDataPath is prioritized, the script writer can verify playback using only the executable file without Unity Editor.
- Commands for messages, characters, backgrounds are provided by default. (Unimplemented) Music, Sound
- Game developers can extend commands by creating components.
- (Unimplemented) Supports Unity.Localization.

# Install with UPM
```
https://github.com/eviltwo/MiniNovel.git?path=MiniNovel/Assets/MiniNovel
```

# Example
https://github.com/eviltwo/MiniNovel/blob/4afed6a56f07d1c6140112e980560b917fdf64d0/MiniNovel/Assets/StreamingAssets/scenario/test.txt#L1-L29
