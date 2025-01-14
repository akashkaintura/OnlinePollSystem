#!/bin/bash

# Validate project structure and dependencies
validate_project() {
    REQUIRED_DIRS=(
        "src/Backend"
        "src/Frontend"
        "src/Mobile"
        "tests"
    )

    for dir in "${REQUIRED_DIRS[@]}"; do
        if [ ! -d "$dir" ]; then
            echo "❌ Missing directory: $dir"
            exit 1
        fi
    done

    # Check .NET SDK
    if ! command -v dotnet &> /dev/null; then
        echo "❌ .NET SDK not installed"
        exit 1
    fi

    # Check Node.js
    if ! command -v node &> /dev/null; then
        echo "❌ Node.js not installed"
        exit 1
    fi

    echo "✅ Project structure and dependencies are valid!"
}

# Run validation
validate_project