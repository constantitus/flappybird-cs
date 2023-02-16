#!/bin/sh
echo -ne '\033c\033]0;flappybird\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/flappybird.x86_64" "$@"
