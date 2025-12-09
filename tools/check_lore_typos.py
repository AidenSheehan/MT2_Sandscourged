#!/usr/bin/env python3
import json
import glob
import io
import re
import subprocess
import shutil
import sys

WORD_RE = re.compile(r"[A-Za-z][A-Za-z'\-]*")

def misspelled_words(text, aspell_path):
    if not aspell_path:
        return []
    words = "\n".join(sorted(set(w.lower() for w in WORD_RE.findall(text))))
    proc = subprocess.run([aspell_path, "list", "--lang=en"], input=words, text=True, capture_output=True)
    if proc.returncode not in (0, 1):  # aspell returns 1 when there are misspelled words
        return []
    return sorted(set(proc.stdout.splitlines()))

def extract_text(entry):
    if isinstance(entry, str):
        return entry
    if isinstance(entry, dict):
        for k in ("english", "English"):
            if k in entry and isinstance(entry[k], str):
                return entry[k]
    return None

def main():
    aspell = shutil.which("aspell")
    found_any = False
    for path in glob.glob("**/*.json", recursive=True):
        try:
            with io.open(path, "r", encoding="utf-8") as f:
                data = json.load(f)
        except Exception:
            continue
        tips = data.get("lore_tooltips")
        if not tips or not isinstance(tips, list):
            continue
        for idx, entry in enumerate(tips):
            text = extract_text(entry)
            if not text:
                continue
            found_any = True
            miss = misspelled_words(text, aspell)
            print(f"{path}:{idx}: {text}")
            if miss:
                print(f"  Misspelled: {', '.join(miss)}")
            else:
                print("  OK")
    if not found_any:
        print("No lore_tooltips found in any JSON files.", file=sys.stderr)
        return 2
    return 0

if __name__ == "__main__":
    raise SystemExit(main())