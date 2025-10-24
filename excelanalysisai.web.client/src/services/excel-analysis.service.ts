// TypeScript types and interfaces matching the backend DTOs
export type ReasoningEffortLevel = 'Low' | 'Medium' | 'High';

export const REASONING_EFFORT_LEVELS = [
	{ level: 'Low' as ReasoningEffortLevel, tokens: 200 },
	{ level: 'Medium' as ReasoningEffortLevel, tokens: 1000 },
	{ level: 'High' as ReasoningEffortLevel, tokens: 5000 },
];

export interface ExcelAnalysisQueryDto {
	modelType: string;
	datasetName: string;
	question: string;
	reasoningLevel: ReasoningEffortLevel;
}

export interface OpenAIQueryCost {
	inputTokenCount: number;
	outputTokenCount: number;
	reasoningTokenCount: number;
	totalCost: number;
}

export interface AIRequestResponseInfo {
	request: string | null;
	response: string;
	cost: OpenAIQueryCost | null;
	isSynthetic: boolean;
}

export interface AIQueryResult {
	userQuery: string;
	requests: AIRequestResponseInfo[];
}

/**
 * Execute an Excel query using AI analysis
 * @param queryDto The query parameters including model, dataset, and user query
 * @returns Promise with the AI analysis result
 */
export async function executeExcelQuery(
	queryDto: ExcelAnalysisQueryDto
): Promise<AIQueryResult> {
	const response = await fetch(`/api/ExcelAnalysis`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
		body: JSON.stringify(queryDto),
	});

	if (!response.ok) {
		const errorBody = await response.text();
		throw new Error(errorBody || `HTTP error! status: ${response.status}`);
	}

	const result: AIQueryResult = await response.json();
	return result;
}
