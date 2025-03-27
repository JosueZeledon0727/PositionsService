module.exports = {
    preset: 'react-app',  // Utiliza el preset de create-react-app para proyectos react
    testEnvironment: 'jsdom',  // Especifica que las pruebas se ejecutarán en un entorno de navegador simulado (jsdom)
    transform: {
        '^.+\\.(js|jsx|ts|tsx)$': 'babel-jest',  // Usa babel-jest para transformar los archivos js/tsx/ts/jsx
    },
    moduleNameMapper: {
        '^react-router-dom$': '<rootDir>/node_modules/react-router-dom',
    },
    setupFilesAfterEnv: ['<rootDir>/src/setupTests.ts'],  // Si tienes configuración extra para las pruebas, como las pruebas en `setupTests.ts`
    transformIgnorePatterns: [
        '/node_modules/(?!(@testing-library|react-router-dom|axios)/).+\\.js$',
    ],
    // Otras configuraciones necesarias
};