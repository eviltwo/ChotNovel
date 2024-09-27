# MiniNovel
Unity向けの最小限のノベルゲームシステムです。
Minimal novel engine for Unity.

# Feature (Japanese)
- スクリプトの構造はKAG(吉里吉里)を参考にしています。
- スクリプトはtxt形式で、streamingAssetsに格納します。
- 実行ファイルだけでtxtの再生確認ができます。スクリプト担当者はUnityエディタを開く必要はありません。
- メッセージ・キャラ・背景のコマンドはデフォルトで用意されています。(未実装)BGM・SE
- ゲーム開発者は、コンポーネントを作成することでコマンドを拡張できます。
- (未実装)Unity.Localizationに対応しています。

# Feature (English)
- The script structure is based on KAG (KiriKiri).
- The script will be in txt format and stored in streamingAssets.
- You can verify the playback of the txt file with just the executable. The script developer does not need to open the Unity editor.
- Commands for messages, characters, backgrounds are provided by default. (Unimplemented) Music, Sound
- Game developers can extend commands by creating components.
- (Unimplemented) Supports Unity.Localization.

# Install with UPM
```
https://github.com/eviltwo/MiniNovel.git?path=MiniNovel/Assets/MiniNovel
```

# Example
https://github.com/eviltwo/MiniNovel/blob/4afed6a56f07d1c6140112e980560b917fdf64d0/MiniNovel/Assets/StreamingAssets/scenario/test.txt#L1-L29
