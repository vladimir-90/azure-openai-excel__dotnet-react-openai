import type {
	AIQueryResult,
	ExcelAnalysisQueryDto,
} from './services/excel-analysis.service';
import {
	getAvailableAiModels,
	getTestDataSets,
} from './services/generation-settings.service';
import { useEffect, useState } from 'react';

import AIAnalysisResult from './components/AnalysisResult';
import type { AIModelDto } from './services/generation-settings.service';
import QuestionForm from './components/QuestionForm';
import type { ReasoningEffortLevel } from './services/excel-analysis.service';
import Toast from './components/base/Toast';
import { executeExcelQuery } from './services/excel-analysis.service';
import { useToastStore } from './stores/toastStore';

function App() {
	const [aiModels, setAiModels] = useState<AIModelDto[]>([]);
	const [testDataSets, setTestDataSets] = useState<string[]>([]);
	const [analysisResult, setAnalysisResult] = useState<AIQueryResult | null>(
		null
	);
	const [queryInputs, setQueryInputs] = useState<{
		modelType: string;
		reasoningLevel: ReasoningEffortLevel;
		datasetName: string;
	} | null>(null);
	const [analyzing, setAnalyzing] = useState<boolean>(false);
	const showToast = useToastStore((state) => state.showToast);

	useEffect(() => {
		getAvailableAiModels().then((models) => setAiModels(models));
		getTestDataSets().then((dataSets) => setTestDataSets(dataSets));
	}, []);

	const handleAnalyze = async (queryData: ExcelAnalysisQueryDto) => {
		setAnalyzing(true);
		try {
			const result = await executeExcelQuery(queryData);
			setAnalysisResult(result);
			setQueryInputs({
				modelType: queryData.modelType,
				reasoningLevel: queryData.reasoningLevel,
				datasetName: queryData.datasetName,
			});
		} catch (error) {
			let errorMessage = 'Analysis failed. Please try again.';
			if (error instanceof Error && !!error.message) {
				errorMessage = error.message;
			}
			showToast(errorMessage, 'error');
		} finally {
			setAnalyzing(false);
		}
	};

	if (analysisResult && queryInputs) {
		return (
			<AIAnalysisResult
				result={analysisResult}
				inputs={queryInputs}
				onAskNewQuestion={() => setAnalysisResult(null)}
			/>
		);
	}

	return (
		<div
			className="container d-flex justify-content-center align-items-center"
			style={{ minHeight: '100vh' }}
		>
			<Toast />
			<QuestionForm
				aiModels={aiModels}
				testDataSets={testDataSets}
				onAnalyze={handleAnalyze}
				showLoading={analyzing}
			/>
		</div>
	);
}

export default App;
