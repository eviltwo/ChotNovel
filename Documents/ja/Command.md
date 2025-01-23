# コマンドの書き方
`[コマンド名 変数=値]` という形で書きます。
```
[actor name=A texture=A]
```

コマンド名が同じ場合は変数を連結して書くことができます。
```
[actor name=A texture=A]
[actor name=A layout=center]
  ↓
[actor name=A texture=A layout=center]
```
