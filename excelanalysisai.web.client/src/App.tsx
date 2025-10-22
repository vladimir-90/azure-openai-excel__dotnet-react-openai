import {
	getAvailableAiModels,
	getTestDataSets,
} from './services/generation-settings.service';
import type { AIModelDto } from './services/generation-settings.service';
import { executeExcelQuery } from './services/excel-analysis.service';
import type {
	ExcelAnalysisQueryDto,
	AIQueryResult,
} from './services/excel-analysis.service';
import { useEffect, useState } from 'react';

import QuestionForm from './components/QuestionForm';
import AIAnalysisResult from './components/AnalysisResult';
import Spinner from './components/base/Spinner';

function App() {
	const [aiModels, setAiModels] = useState<AIModelDto[]>([]);
	const [testDataSets, setTestDataSets] = useState<string[]>([]);
	const [analysisResult, setAnalysisResult] = useState<AIQueryResult | null>(
		null
	);
	const [analyzing, setAnalyzing] = useState<boolean>(false);

	useEffect(() => {
		loadData();
	}, []);

	const loadData = async () => {
		getAvailableAiModels().then((models) => setAiModels(models));
		getTestDataSets().then((dataSets) => setTestDataSets(dataSets));
	};

	const handleAnalyze = async (queryData: ExcelAnalysisQueryDto) => {
		setAnalyzing(true);
		try {
			const result = await executeExcelQuery(queryData);
			setAnalysisResult(result);
		} catch (error) {
			console.error('Analysis failed:', error);
			// Handle error - could set error state here
		} finally {
			setAnalyzing(false);
		}
	};

	if (analyzing) {
		return (
			<div
				className="container d-flex justify-content-center align-items-center"
				style={{ minHeight: '100vh' }}
			>
				<div className="p-3">
					<div className="mb-3">
						<Spinner />
					</div>
					Analyzing your data...
				</div>
			</div>
		);
	}

	if (analysisResult) {
		return <AIAnalysisResult result={analysisResult} />;
	}

	return (
		<div
			className="container d-flex justify-content-center align-items-center"
			style={{ minHeight: '100vh' }}
		>
			<QuestionForm
				aiModels={aiModels}
				testDataSets={testDataSets}
				onAnalyze={handleAnalyze}
			/>
		</div>
	);
}

export default App;
