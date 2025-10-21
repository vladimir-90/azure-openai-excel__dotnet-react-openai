/**
 * Service for handling Generation Settings API calls
 */

/**
 * Fetches available AI models from the API
 * @returns {Promise<string[]>} Array of available AI model names
 */
export async function getAvailableAiModels(): Promise<string[]> {
	try {
		const response = await fetch(
			'/api/GenerationSettings/available-ai-models'
		);
		if (!response.ok) {
			throw new Error(
				`Failed to fetch AI models: ${response.status} ${response.statusText}`
			);
		}

		const text = await response.text();
		console.log('Raw AI models response:', text);

		if (!text || text.trim() === '') {
			console.warn('Empty response for AI models, returning default');
			return ['GPT-4-nano']; // fallback
		}

		return JSON.parse(text);
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

		const text = await response.text();
		console.log('Raw test data sets response:', text);

		if (!text || text.trim() === '') {
			console.warn(
				'Empty response for test data sets, returning default'
			);
			return ['employees-10']; // fallback
		}

		return JSON.parse(text);
	} catch (error) {
		console.error('Error fetching test data sets:', error);
		throw error;
	}
}
