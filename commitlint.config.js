module.exports = {
    extends: ['@commitlint/config-conventional'],
    rules: {
        "subject-case": [0, 'never'],
        "subject-max-length": [2, "always", 100],
        "scope-enum": _ => [
            2,
            "always",
            [
                "deps",
                "docs",
                "core"
            ]
        ]
    }
}
