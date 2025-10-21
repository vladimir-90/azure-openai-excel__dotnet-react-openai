/**
 * Service for handling Generation Settings API calls
 */

// TypeScript interfaces matching the backend DTOs
export interface AIModelDto {
	modelType: string;
	label: string;
	pricing: {
		input: number;
		cachedInput: number;
		output: number;
	};
}

export interface DataSetInfoDto {
	name: string;
	schema: string;
	dataSlice: string[][];
	totalEntityCount: number;
}

/**
 * Fetches available AI models from the API
 * @returns {Promise<AIModelDto[]>} Array of available AI model DTOs
 */
export async function getAvailableAiModels(): Promise<AIModelDto[]> {
	try {
		const response = await fetch(
			'/api/GenerationSettings/available-ai-models'
		);
		if (!response.ok) {
			throw new Error(
				`Failed to fetch AI models: ${response.status} ${response.statusText}`
			);
		}

		return await response.json();
	} catch (error) {
		console.error('Error fetching available AI models:', error);
		throw error;
	}
}

/**
 * Fetches available test data sets from the API
 * @returns {Promise<string[]>} Array of available test data set names
 */
export async function getTestDataSets(): Promise<string[]> {
	try {
		const response = await fetch('/api/GenerationSettings/test-data-sets');
		if (!response.ok) {
			throw new Error(
				`Failed to fetch test data sets: ${response.status} ${response.statusText}`
			);
		}

		return await response.json();
	} catch (error) {
		console.error('Error fetching test data sets:', error);
		throw error;
	}
}

/**
 * Fetches detailed information about a specific test dataset
 * @param dataSetName The name of the dataset to fetch
 * @returns {Promise<DataSetInfoDto>} Dataset information including schema and data slice
 */
export async function getTestDataset(
	dataSetName: string
): Promise<DataSetInfoDto> {
	try {
		const response = await fetch(
			`/api/GenerationSettings/test-data-sets/${encodeURIComponent(
				dataSetName
			)}`
		);
		if (!response.ok) {
			throw new Error(
				`Failed to fetch dataset '${dataSetName}': ${response.status} ${response.statusText}`
			);
		}

		return await response.json();
	} catch (error) {
		console.error(`Error fetching dataset '${dataSetName}':`, error);
		throw error;
	}
}
