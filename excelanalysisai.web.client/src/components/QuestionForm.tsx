import { useState, useEffect } from 'react';
import type {
	AIModelDto,
	DataSetInfoDto,
} from '../services/generation-settings.service';
import { getTestDataset } from '../services/generation-settings.service';
import type { ExcelAnalysisQueryDto } from '../services/excel-analysis.service';
import AiModelPricing from './AiModelPricing';
import DatasetInfo from './DatasetInfo';
import LazySelect from './base/LazySelect';

interface QuestionFormProps {
	aiModels: AIModelDto[];
	testDataSets: string[];
	onAnalyze: (queryData: ExcelAnalysisQueryDto) => void;
}

function QuestionForm({
	aiModels,
	testDataSets,
	onAnalyze,
}: QuestionFormProps) {
	const [selectedModel, setSelectedModel] = useState<string>('');
	const [selectedDataSet, setSelectedDataSet] = useState<string>('');
	const [question, setQuestion] = useState<string>('');
	const [datasetInfo, setDatasetInfo] = useState<DataSetInfoDto | null>(null);
	const [loadingDataset, setLoadingDataset] = useState<boolean>(false);

	const selectedModelData = aiModels.find(
		(model) => model.modelType === selectedModel
	);

	// Load dataset info when dataset is selected
	useEffect(() => {
		if (selectedDataSet) {
			setLoadingDataset(true);
			setDatasetInfo(null);

			getTestDataset(selectedDataSet)
				.then((info) => {
					setDatasetInfo(info);
				})
				.catch((error) => {
					console.error('Failed to load dataset info:', error);
				})
				.finally(() => {
					setLoadingDataset(false);
				});
		} else {
			setDatasetInfo(null);
		}
	}, [selectedDataSet]);

	return (
		<div className="card border-0 shadow-lg question-form-card">
			{/* Header */}
			<div className="card-header question-form-header text-white text-center py-3 border-0">
				<h4 className="mb-0 fw-light">Excel Analysis AI</h4>
				<small className="opacity-75">
					Ask questions about your data
				</small>
			</div>

			{/* Form Body */}
			<div className="card-body p-4">
				<div className="row g-3">
					{/* Model Selection */}
					<div className="col-md-6">
						<LazySelect
							label="🤖 AI Model"
							placeholder="Choose model..."
							value={selectedModel}
							onChange={setSelectedModel}
							loading={aiModels.length === 0}
							options={aiModels.map((model) => ({
								value: model.modelType,
								label: model.label,
							}))}
						/>
					</div>

					{/* Dataset Selection */}
					<div className="col-md-6">
						<LazySelect
							label="📊 Dataset"
							placeholder="Choose dataset..."
							value={selectedDataSet}
							onChange={setSelectedDataSet}
							loading={testDataSets.length === 0}
							options={testDataSets.map((dataSet) => ({
								value: dataSet,
								label: dataSet,
							}))}
						/>
					</div>

					{/* Pricing Display */}
					<div className="col-12 m-0">
						<AiModelPricing selectedModel={selectedModelData} />
					</div>

					{/* Dataset Information Display */}
					<div className="col-12 m-0">
						<DatasetInfo
							datasetInfo={datasetInfo}
							loading={loadingDataset}
						/>
					</div>

					{/* Question Input */}
					<div className="col-12 mt-4">
						<div className="form-floating">
							<textarea
								id="question-textarea"
								value={question}
								onChange={(e) => setQuestion(e.target.value)}
								className="form-control question-form-textarea"
								placeholder="What would you like to know about your data?"
								rows={3}
							/>
							<label
								htmlFor="question-textarea"
								className="text-muted"
							>
								💭 Your Question
							</label>
						</div>
						<div className="form-text text-muted mt-2">
							Try asking: "What are the top 5 sales by region?" or
							"Show me the average salary by department"
						</div>
					</div>
				</div>
			</div>

			{/* Footer with Action Button */}
			<div className="card-footer bg-light border-0 text-end p-4">
				<button
					onClick={() =>
						onAnalyze({
							modelType: selectedModel,
							datasetName: selectedDataSet,
							question: question.trim(),
						})
					}
					className={`btn btn-lg px-5 fw-semibold question-form-button ${
						selectedModel && selectedDataSet && question.trim()
							? 'question-form-button-enabled'
							: 'question-form-button-disabled'
					}`}
					disabled={
						!selectedModel || !selectedDataSet || !question.trim()
					}
				>
					{!selectedModel || !selectedDataSet || !question.trim() ? (
						<span>Complete the form above</span>
					) : (
						<span className="text-white">🚀 Analyze Data</span>
					)}
				</button>
			</div>
		</div>
	);
}

export default QuestionForm;
