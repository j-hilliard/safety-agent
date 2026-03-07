module.exports = {
    'rules': {},
    'env': { 'browser': true, 'es2021': true, 'node': true },
    'plugins': ['@typescript-eslint', 'vue'],
    'extends': ['eslint:recommended', 'plugin:@typescript-eslint/recommended', 'plugin:vue/vue3-essential'],
    'parserOptions': { 'ecmaVersion': 'latest', 'parser': '@typescript-eslint/parser', 'sourceType': 'module' },
    'overrides': [{
        'env': { 'node': true },
        'files': ['.eslintrc.{js,cjs}'],
        'parserOptions': { 'sourceType': 'script' },
    }],
};
