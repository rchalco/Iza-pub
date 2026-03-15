import type { Config } from 'jest';

const config: Config = {
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
  testEnvironment: 'jsdom',
  testMatch: ['**/src/**/*.spec.ts'],
  coverageDirectory: 'coverage',
  collectCoverageFrom: [
    'src/**/*.ts',
    '!src/**/*.spec.ts',
    '!src/main.ts',
    '!src/polyfills.ts',
    '!src/environments/*.ts',
  ],
  moduleNameMapper: {
    '^src/(.*)$': '<rootDir>/src/$1',
    '^@preact/signals-core$':
      '<rootDir>/node_modules/@preact/signals-core/dist/signals-core.js',
  },
  transformIgnorePatterns: [
    'node_modules/(?!(.*\\.mjs$|@ionic/core|ionicons))',
  ],
  transform: {
    '^.+\\.(ts|js|mjs|html|svg)$': [
      'jest-preset-angular',
      {
        tsconfig: '<rootDir>/tsconfig-jest.json',
        stringifyContentPathRegex: '\\.(html|svg)$',
      },
    ],
  },
};

export default config;
