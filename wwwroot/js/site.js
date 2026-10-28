// ============================================================================
// HOÀNG VIỆT (.NET 10 LEARNING PLATFORM) — NEXT-GEN CYBER-HUD EDITION
// 1. Live Pipeline Simulator (Auto-Play, Prev/Next, Line-Numbered Cyber-IDE)
// 2. Interactive Line-Hover Sync (Code Anatomy <-> C# Editor) & Bento Switcher
// 3. Live Sidebar Search Filter & Mouse-Tracking Spotlight Cards
// 4. Gamified Quiz Arena + Instant SQL Server & Chart.js Telemetry Update
// 5. 2D/3D-Ready AI Mascot (Dual-Mode + IMemoryCache 24h)
// ============================================================================

(function () {
    let homeRadarChartInstance = null;
    let homeTimelineChartInstance = null;
    let autoPlayIntervalId = null;

    // ------------------------------------------------------------------------
    // 1. MODULE HỘP BƯỚC TƯƠNG TÁC, PIPELINE SIMULATOR & CYBER-IDE
    // ------------------------------------------------------------------------
    function escapeHtml(str) {
        return (str || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function formatFlowDetailHtml(stepDetail, stepTitle, stepCode) {
        const raw = (stepDetail || '').trim();
        let inputPart = '';
        let processPart = '';
        let outputPart = '';

        // 1. Check format with emojis: 📥 Đầu vào: ... ⚙️ Cơ chế ...: ... 📤 Đầu ra: ...
        const emojiMatch = raw.match(/📥\s*Đầu vào:\s*([\s\S]*?)\s*⚙️\s*Cơ chế[^:]*:\s*([\s\S]*?)\s*📤\s*Đầu ra:\s*([\s\S]*)/i);
        if (emojiMatch) {
            inputPart = emojiMatch[1].trim();
            processPart = emojiMatch[2].trim();
            outputPart = emojiMatch[3].trim();
        } else {
            // 2. Check format: Đầu vào: ... -> Xử lý: ... -> Đẩy kết quả / Đầu ra: ...
            const arrowMatch = raw.match(/Đầu vào:\s*([\s\S]*?)\s*(?:->|➔)\s*Xử lý:\s*([\s\S]*?)\s*(?:->|➔)\s*(?:Đẩy kết quả|Đầu ra):\s*([\s\S]*)/i);
            if (arrowMatch) {
                inputPart = arrowMatch[1].trim();
                processPart = arrowMatch[2].trim();
                outputPart = arrowMatch[3].trim();
            }
        }

        if (inputPart && processPart && outputPart) {
            return `
                <div class="hv-flow-stage-card stage-input">
                    <div class="hv-flow-stage-label">📥 Đầu vào (Input)</div>
                    <div class="hv-flow-stage-body">${escapeHtml(inputPart)}</div>
                </div>
                <div class="hv-flow-stage-card stage-process">
                    <div class="hv-flow-stage-label">⚙️ Cơ chế hoạt động ngầm (Under the Hood)</div>
                    <div class="hv-flow-stage-body">${escapeHtml(processPart)}</div>
                </div>
                <div class="hv-flow-stage-card stage-output">
                    <div class="hv-flow-stage-label">📤 Đầu ra (Output)</div>
                    <div class="hv-flow-stage-body">${escapeHtml(outputPart)}</div>
                </div>`;
        }

        const cleanTitle = (stepTitle || 'Bước thực thi').replace(/^\d+\.\s*/, '');
        const firstCodeLine = ((stepCode || '').split('\n').find(l => l.trim() && !l.trim().startsWith('//')) || '').trim();

        return `
            <div class="hv-flow-stage-card stage-input">
                <div class="hv-flow-stage-label">📥 Đầu vào (Input)</div>
                <div class="hv-flow-stage-body">Bối cảnh thực thi: <strong>${escapeHtml(cleanTitle)}</strong>${firstCodeLine ? ` (<code>${escapeHtml(firstCodeLine.substring(0, 55))}${firstCodeLine.length > 55 ? '...' : ''}</code>)` : ''}</div>
            </div>
            <div class="hv-flow-stage-card stage-process">
                <div class="hv-flow-stage-label">⚙️ Cơ chế hoạt động ngầm (Under the Hood)</div>
                <div class="hv-flow-stage-body">${escapeHtml(raw).replace(/\s*(?:->|➔)\s*/g, ' <span class="hv-inline-arrow">➔</span> ')}</div>
            </div>
            <div class="hv-flow-stage-card stage-output">
                <div class="hv-flow-stage-label">📤 Đầu ra (Output)</div>
                <div class="hv-flow-stage-body">Hoàn tất mắt xích <strong>${escapeHtml(cleanTitle)}</strong> và sẵn sàng chuyển tiếp sang bước kế tiếp trong luồng.</div>
            </div>`;
    }

    function highlightCSharpCode(rawCode) {
        if (!rawCode) return '';
        const lines = rawCode.split('\n');
        const kwRegex = /\b(using|namespace|public|private|protected|internal|static|readonly|const|sealed|abstract|virtual|override|partial|class|record|struct|interface|enum|void|int|long|double|decimal|bool|string|char|var|object|new|return|if|else|switch|case|default|for|foreach|while|do|in|break|continue|try|catch|finally|throw|async|await|out|ref|params|this|base|true|false|null|from|where|orderby|select|get|set|init|with)\b/g;
        const typeRegex = /\b(List|Dictionary|HashSet|Queue|Stack|ArrayList|KeyValuePair|Func|Action|Predicate|Task|IEnumerable|IQueryable|Guid|DateTime|TimeSpan|Console|Math|Convert|StringBuilder|Exception|ArgumentException|InvalidOperationException|ControllerBase|IActionResult|HttpContext|WebApplication)\b/g;

        return lines.map((line, index) => {
            let commentPart = '';
            let codePart = line;

            let inQuotes = false;
            for (let i = 0; i < line.length - 1; i++) {
                if (line[i] === '"' && (i === 0 || line[i - 1] !== '\\')) {
                    inQuotes = !inQuotes;
                }
                if (!inQuotes && line[i] === '/' && line[i + 1] === '/') {
                    codePart = line.substring(0, i);
                    commentPart = line.substring(i);
                    break;
                }
            }

            const strings = [];
            let tokenized = codePart.replace(/\$?"(?:\\.|[^"\\])*"/g, m => {
                const idx = strings.length;
                strings.push(`<span class="cs-str">${escapeHtml(m)}</span>`);
                return `__STR_TOK_${idx}__`;
            });

            tokenized = escapeHtml(tokenized);
            tokenized = tokenized.replace(kwRegex, '<span class="cs-kw">$1</span>');
            tokenized = tokenized.replace(typeRegex, '<span class="cs-type">$1</span>');
            tokenized = tokenized.replace(/\b(\d+(?:\.\d+)?m?)\b/g, '<span class="cs-num">$1</span>');

            for (let i = 0; i < strings.length; i++) {
                tokenized = tokenized.replace(`__STR_TOK_${i}__`, strings[i]);
            }

            if (commentPart) {
                tokenized += `<span class="cs-comment">${escapeHtml(commentPart)}</span>`;
            }

            const lineNum = index + 1;
            const formattedNum = lineNum < 10 ? `0${lineNum}` : `${lineNum}`;
            return `<div class="hv-code-line-row" data-code-line="${lineNum}"><span class="hv-line-gutter">${formattedNum}</span><span>${tokenized || ' '}</span></div>`;
        }).join('');
    }

    function explainSingleCSharpLine(line) {
        const trimmed = line.trim();
        if (trimmed.startsWith('//')) {
            return `Chú thích giải thích: ${trimmed.replace(/^\/\/\s*/, '')}`;
        }

        const commentIdx = trimmed.indexOf('//');
        if (commentIdx > 0) {
            const note = trimmed.substring(commentIdx + 2).trim();
            if (note) return note;
        }

        if (/^using\s+/.test(trimmed)) return 'Nạp thư viện (Namespace) để sử dụng các lớp và công cụ có sẵn của .NET.';
        if (/^namespace\s+/.test(trimmed)) return 'Định nghĩa không gian tên (Namespace) gom nhóm các Class liên quan.';
        if (/^(public|internal|private|protected)?\s*(sealed|abstract|static|partial)?\s*(class|record|interface|enum|struct)\s+/.test(trimmed)) {
            return 'Khai báo cấu trúc kiểu dữ liệu (Class / Record / Interface / Enum).';
        }
        if (/=>/.test(trimmed)) return 'Sử dụng biểu thức mũi tên (Lambda / Expression-bodied) để xử lý hoặc trả về kết quả ngắn gọn.';
        if (/\.Where\(/.test(trimmed)) return 'Lọc các phần tử thỏa mãn điều kiện bằng toán tử LINQ .Where().';
        if (/\.OrderBy|\.ThenBy/.test(trimmed)) return 'Sắp xếp thứ tự các phần tử theo tiêu chí chỉ định.';
        if (/\.SelectMany\(/.test(trimmed)) return 'Làm phẳng (Flatten) các danh sách con thành một danh sách duy nhất.';
        if (/\.Select\(/.test(trimmed)) return 'Biến đổi / chiếu (Project) từng phần tử sang dạng dữ liệu hoặc DTO mới.';
        if (/\.ToList\(|\.ToArray\(|\.ToDictionary\(/.test(trimmed)) return 'Thực thi ngay câu truy vấn (Immediate Execution) và đóng gói kết quả.';
        if (/\.FirstOrDefault\(|\.SingleOrDefault\(/.test(trimmed)) return 'Tìm kiếm và trả về 1 phần tử khớp điều kiện (hoặc null nếu không tìm thấy).';
        if (/\.GroupBy\(/.test(trimmed)) return 'Gom nhóm các phần tử có chung giá trị khóa (Key).';
        if (/\.TryGetValue\(/.test(trimmed)) return 'Tra cứu an toàn trong Dictionary với tốc độ O(1) mà không gây lỗi KeyNotFoundException.';
        if (/TryParse\(/.test(trimmed)) return 'Ép kiểu an toàn từ chuỗi sang số, trả về true/false thay vì làm sập chương trình.';
        if (/Console\.Write/.test(trimmed)) return 'Hiển thị kết quả đầu ra lên màn hình Console.';
        if (/^return\b/.test(trimmed)) return 'Trả kết quả về cho nơi đã gọi phương thức.';
        if (/^(if|else if|else|switch)\b/.test(trimmed)) return 'Kiểm tra điều kiện logic để quyết định nhánh code được thực thi.';
        if (/^(for|foreach|while|do)\b/.test(trimmed)) return 'Thực hiện vòng lặp duyệt qua các phần tử hoặc lặp theo điều kiện.';
        if (/\bnew\b/.test(trimmed)) return 'Khởi tạo đối tượng mới trên vùng nhớ và gán vào biến.';
        if (/=/.test(trimmed)) return 'Tính toán biểu thức bên phải và gán giá trị vào biến bên trái.';
        return 'Thực thi câu lệnh xử lý trong luồng hiện tại.';
    }

    function buildCodeAnatomyHtml(rawCode) {
        if (!rawCode) return '';
        const lines = rawCode.split('\n');
        const items = [];

        lines.forEach((line, idx) => {
            const trimmed = line.trim();
            if (!trimmed || trimmed === '{' || trimmed === '}' || trimmed === '};' || trimmed === ');') return;
            const lineNum = idx + 1;
            items.push(`
                <div class="hv-code-anatomy-row"
                     data-target-line="${lineNum}"
                     onmouseenter="window.HoangVietFlow.highlightLine(${lineNum}, true)"
                     onmouseleave="window.HoangVietFlow.highlightLine(${lineNum}, false)">
                    <span class="hv-code-line-badge font-mono">Dòng ${lineNum}</span>
                    <div class="hv-code-anatomy-content">
                        <code class="hv-code-anatomy-snippet">${escapeHtml(trimmed)}</code>
                        <div class="hv-code-anatomy-desc">➔ ${escapeHtml(explainSingleCSharpLine(trimmed))}</div>
                    </div>
                </div>`);
        });

        if (items.length === 0) return '';
        return `
            <div class="hv-code-anatomy-header">
                <i class="bi bi-search me-1 text-info"></i>Giải thích chi tiết từng dòng lệnh (Di chuột vào từng dòng để đối chiếu trên IDE)
            </div>
            <div class="hv-code-anatomy-list">
                ${items.join('')}
            </div>`;
    }

    window.HoangVietFlow = {
        toggleMobileSidebar: function () {
            const sidebar = document.getElementById('coursePlayerSidebar');
            if (!sidebar) return;
            sidebar.classList.toggle('mobile-collapsed');
        },

        filterSidebar: function (query) {
            const q = (query || '').trim().toLowerCase();
            const topicItems = document.querySelectorAll('#sidebarTopicAccordion [data-topic-item]');
            topicItems.forEach(topicEl => {
                const links = topicEl.querySelectorAll('.cp-sublesson-link');
                let hasVisible = false;
                links.forEach(link => {
                    const text = (link.getAttribute('data-search-text') || link.textContent || '').toLowerCase();
                    const matched = !q || text.includes(q);
                    link.style.display = matched ? '' : 'none';
                    if (matched) hasVisible = true;
                });
                topicEl.style.display = hasVisible ? '' : 'none';

                const collapseEl = topicEl.querySelector('.accordion-collapse');
                if (q && hasVisible && collapseEl && !collapseEl.classList.contains('show')) {
                    collapseEl.classList.add('show');
                }
            });
        },

        toggleBentoLayout: function () {
            const grid = document.querySelector('#theoryBentoContainer .hv-bento-grid');
            if (!grid) return;
            grid.classList.toggle('bento-single-col');
        },

        highlightLine: function (lineNum, active) {
            const row = document.querySelector(`#activeStepCode .hv-code-line-row[data-code-line="${lineNum}"]`);
            if (!row) return;
            row.classList.toggle('line-highlighted', !!active);
        },

        selectStep: function (buttonEl, fromAutoPlay) {
            if (!buttonEl) return;
            if (!fromAutoPlay && autoPlayIntervalId) {
                this.stopAutoPlay();
            }

            const allBoxes = document.querySelectorAll('#interactiveFlowChain .flow-step-box');
            allBoxes.forEach(b => b.classList.remove('active'));
            buttonEl.classList.add('active');

            const stepNumber = buttonEl.getAttribute('data-step-number') || '1';
            const stepTitle = buttonEl.getAttribute('data-step-title') || '';
            const stepDetail = buttonEl.getAttribute('data-step-detail') || '';
            const stepCode = buttonEl.getAttribute('data-step-code') || '';

            const inspectorEl = document.getElementById('flowStepInspector');
            const badgeEl = document.getElementById('activeStepBadge');
            const titleEl = document.getElementById('activeStepTitle');
            const detailEl = document.getElementById('activeStepDetail');
            const codeEl = document.getElementById('activeStepCode');
            const anatomyEl = document.getElementById('activeStepCodeAnatomy');
            const fileLabelEl = document.getElementById('activeCodeFileLabel');

            if (inspectorEl) {
                inspectorEl.classList.remove('hv-step-switching');
                void inspectorEl.offsetWidth;
                inspectorEl.classList.add('hv-step-switching');
            }

            if (badgeEl) badgeEl.textContent = `ĐANG XEM: BƯỚC ${stepNumber}`;
            if (titleEl) titleEl.textContent = stepTitle;
            if (detailEl) detailEl.innerHTML = formatFlowDetailHtml(stepDetail, stepTitle, stepCode);
            if (codeEl) {
                codeEl.setAttribute('data-raw-code', stepCode);
                codeEl.innerHTML = highlightCSharpCode(stepCode);
            }
            if (anatomyEl) {
                anatomyEl.innerHTML = buildCodeAnatomyHtml(stepCode);
            }
            if (fileLabelEl) fileLabelEl.textContent = `Step${stepNumber}_Sample.cs`;
        },

        prevStep: function () {
            const allBoxes = Array.from(document.querySelectorAll('#interactiveFlowChain .flow-step-box'));
            if (allBoxes.length === 0) return;
            const currentIdx = allBoxes.findIndex(b => b.classList.contains('active'));
            const prevIdx = currentIdx > 0 ? currentIdx - 1 : allBoxes.length - 1;
            this.selectStep(allBoxes[prevIdx]);
        },

        nextStep: function (fromAutoPlay) {
            const allBoxes = Array.from(document.querySelectorAll('#interactiveFlowChain .flow-step-box'));
            if (allBoxes.length === 0) return;
            const currentIdx = allBoxes.findIndex(b => b.classList.contains('active'));
            const nextIdx = currentIdx >= 0 && currentIdx < allBoxes.length - 1 ? currentIdx + 1 : 0;
            this.selectStep(allBoxes[nextIdx], fromAutoPlay);
        },

        toggleAutoPlay: function () {
            if (autoPlayIntervalId) {
                this.stopAutoPlay();
                return;
            }
            const btn = document.getElementById('autoPlayFlowBtn');
            if (btn) {
                btn.innerHTML = '<i class="bi bi-pause-fill me-1"></i>Đang mô phỏng (Dừng)';
                btn.classList.remove('btn-hv-orange');
                btn.classList.add('btn-hv-purple');
            }
            autoPlayIntervalId = setInterval(() => {
                this.nextStep(true);
            }, 3200);
        },

        stopAutoPlay: function () {
            if (autoPlayIntervalId) {
                clearInterval(autoPlayIntervalId);
                autoPlayIntervalId = null;
            }
            const btn = document.getElementById('autoPlayFlowBtn');
            if (btn) {
                btn.innerHTML = '<i class="bi bi-play-fill me-1"></i>Tự động chạy luồng';
                btn.classList.remove('btn-hv-purple');
                btn.classList.add('btn-hv-orange');
            }
        },

        copyActiveCode: function () {
            const codeEl = document.getElementById('activeStepCode');
            const btn = document.getElementById('copyCodeBtn');
            if (!codeEl || !navigator.clipboard) return;

            const raw = codeEl.getAttribute('data-raw-code') || codeEl.textContent || '';
            navigator.clipboard.writeText(raw).then(() => {
                if (btn) {
                    const original = btn.innerHTML;
                    btn.innerHTML = '<i class="bi bi-check2 me-1 text-info"></i>Đã Copy!';
                    setTimeout(() => { btn.innerHTML = original; }, 1600);
                }
            });
        },

        getActiveStepTitle: function () {
            const activeBtn = document.querySelector('#interactiveFlowChain .flow-step-box.active');
            if (!activeBtn) return 'Bước 1';
            const num = activeBtn.getAttribute('data-step-number') || '1';
            const title = activeBtn.getAttribute('data-step-title') || '';
            return `Bước ${num}: ${title}`;
        }
    };

    // ------------------------------------------------------------------------
    // 2. MODULE BÀI TẬP KIỂM TRA & CẬP NHẬT CHART.JS TỨC THÌ (PHẦN 3)
    // ------------------------------------------------------------------------
    window.HoangVietQuiz = {
        selectOption: function (labelEl) {
            if (!labelEl) return;
            const allCards = document.querySelectorAll('#quizOptionsList .cp-quiz-option-card');
            allCards.forEach(c => {
                c.classList.remove('selected', 'correct', 'wrong');
            });
            labelEl.classList.add('selected');
            const radio = labelEl.querySelector('input[type="radio"]');
            if (radio) radio.checked = true;
        },

        submitAnswer: async function () {
            const btn = document.getElementById('btnCompleteSubLesson');
            const feedbackBox = document.getElementById('quizFeedbackBox');
            const selectedRadio = document.querySelector('input[name="subLessonQuizOption"]:checked');

            if (!btn || !feedbackBox) return;

            if (!selectedRadio) {
                feedbackBox.className = 'alert alert-warning mb-3';
                feedbackBox.innerHTML = '<i class="bi bi-exclamation-circle-fill me-2"></i>Vui lòng chọn 1 đáp án trước khi bấm Hoàn thành bài học!';
                return;
            }

            const chosenValue = selectedRadio.value.trim();
            const chosenLetter = (selectedRadio.getAttribute('data-letter') || '').trim().toUpperCase();
            const correctAnswer = (btn.getAttribute('data-correct-answer') || '').trim();
            const explanation = btn.getAttribute('data-explanation') || '';
            const subLessonId = parseInt(btn.getAttribute('data-sublesson-id') || '0', 10);
            const currentTrack = btn.getAttribute('data-track') || 'INTERN_TRACK';

            const selectedCard = selectedRadio.closest('.cp-quiz-option-card');

            const isCorrect =
                chosenValue === correctAnswer ||
                chosenLetter === correctAnswer.toUpperCase() ||
                chosenValue.toUpperCase().startsWith(correctAnswer.toUpperCase() + '.');

            if (!isCorrect) {
                if (selectedCard) selectedCard.classList.add('wrong');
                feedbackBox.className = 'alert alert-danger mb-3';
                feedbackBox.innerHTML =
                    `<div class="fw-bold mb-1"><i class="bi bi-x-octagon-fill me-2"></i>Đáp án chưa chính xác!</div>` +
                    `<div class="small">Hãy đọc lại Phần 1 & các Hộp Bước ở Phần 2 rồi chọn lại đáp án đúng để mở khóa điểm kỹ năng nhé.</div>`;
                return;
            }

            if (selectedCard) selectedCard.classList.add('correct');
            btn.disabled = true;

            try {
                const res = await fetch('/Home/CompleteLesson', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        subLessonId: subLessonId,
                        lessonId: subLessonId,
                        currentTrack: currentTrack
                    })
                });

                const data = await res.json();
                if (data && data.success) {
                    feedbackBox.className = 'alert alert-success mb-3';
                    const pointsMsg = data.addedPoints > 0
                        ? `Đã cộng <strong>+${data.addedPoints} EXP</strong> vào kỹ năng <strong>${data.skillName}</strong> (Hiện tại: ${data.updatedSkillScore}%).`
                        : `Bài học này đã được ghi nhận hoàn thành trong Database.`;

                    feedbackBox.innerHTML =
                        `<div class="fw-bold mb-1"><i class="bi bi-check-circle-fill me-2"></i>Chính xác! Chúc mừng Hoàng Việt đã hoàn thành Bài ${data.lessonCode}!</div>` +
                        `<div class="mb-1">${explanation}</div>` +
                        `<div class="small text-info font-mono">${pointsMsg}</div>`;

                    // Cập nhật thanh tiến độ & EXP trên Header
                    const headerText = document.getElementById('headerProgressText');
                    const headerBar = document.getElementById('headerProgressBar');
                    const headerXp = document.getElementById('headerXpBadge');
                    if (headerText) {
                        headerText.textContent = `${data.overallProgressPercent}% (${data.completedLessonsCount}/${data.totalLessonsCount} bài)`;
                    }
                    if (headerBar) {
                        headerBar.style.width = `${data.overallProgressPercent}%`;
                    }
                    if (headerXp && typeof data.completedLessonsCount === 'number') {
                        headerXp.textContent = `⚡ ${data.completedLessonsCount * 10} EXP`;
                    }

                    // Cập nhật trạng thái bài học và icon tích xanh trên Sidebar
                    const statusBadge = document.getElementById('lessonStatusBadge');
                    if (statusBadge) {
                        statusBadge.className = 'badge rounded-pill px-3 py-2 bg-success';
                        statusBadge.innerHTML = '<i class="bi bi-check2-circle me-1"></i><span>Đã hoàn thành</span>';
                    }

                    const sidebarIcon = document.getElementById(`sidebarIcon_${subLessonId}`);
                    if (sidebarIcon) {
                        sidebarIcon.innerHTML = '<i class="bi bi-check-circle-fill text-success"></i>';
                    }

                    // Cập nhật trực tiếp 2 Biểu đồ Chart.js tại Trang chủ
                    if (homeRadarChartInstance && Array.isArray(data.skillScores)) {
                        homeRadarChartInstance.data.datasets[0].data = data.skillScores;
                        homeRadarChartInstance.update();
                    }
                    if (homeTimelineChartInstance && Array.isArray(data.timelineCounts)) {
                        homeTimelineChartInstance.data.datasets[0].data = data.timelineCounts;
                        homeTimelineChartInstance.update();
                    }
                }
            } catch (err) {
                console.error(err);
            } finally {
                btn.disabled = false;
            }
        }
    };

    // ------------------------------------------------------------------------
    // 3. KHỞI TẠO BIỂU ĐỒ NĂNG LỰC (RADAR CHART & TIMELINE LINE CHART) TẠI TRANG CHỦ
    // ------------------------------------------------------------------------
    function initHomeCharts() {
        const chartData = window.HoangVietInitialChartData;
        if (!chartData || !window.Chart) return;

        const radarCanvas = document.getElementById('homeSkillRadarChart');
        if (radarCanvas) {
            homeRadarChartInstance = new Chart(radarCanvas, {
                type: 'radar',
                data: {
                    labels: chartData.skillLabels,
                    datasets: [{
                        label: 'Năng lực hiện tại (%)',
                        data: chartData.skillScores,
                        backgroundColor: 'rgba(0, 240, 255, 0.18)',
                        borderColor: '#00F0FF',
                        pointBackgroundColor: '#A855F7',
                        pointBorderColor: '#FFFFFF',
                        borderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        r: {
                            min: 0,
                            max: 100,
                            ticks: { stepSize: 20, color: '#94A3B8', backdropColor: 'transparent' },
                            grid: { color: 'rgba(56, 189, 248, 0.2)' },
                            angleLines: { color: 'rgba(56, 189, 248, 0.2)' },
                            pointLabels: { color: '#F8FAFC', font: { size: 12, weight: '600' } }
                        }
                    },
                    plugins: {
                        legend: { labels: { color: '#E2E8F0' } }
                    }
                }
            });
        }

        const lineCanvas = document.getElementById('homeTimelineLineChart');
        if (lineCanvas) {
            homeTimelineChartInstance = new Chart(lineCanvas, {
                type: 'line',
                data: {
                    labels: chartData.timelineLabels,
                    datasets: [{
                        label: 'Số bài học con hoàn thành',
                        data: chartData.timelineCounts,
                        borderColor: '#10B981',
                        backgroundColor: 'rgba(16, 185, 129, 0.16)',
                        fill: true,
                        tension: 0.35,
                        pointRadius: 4,
                        pointBackgroundColor: '#00F0FF',
                        pointBorderColor: '#FFFFFF'
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            suggestedMax: 5,
                            ticks: { stepSize: 1, color: '#94A3B8' },
                            grid: { color: 'rgba(148, 163, 184, 0.15)' }
                        },
                        x: {
                            ticks: { color: '#CBD5E1' },
                            grid: { color: 'rgba(148, 163, 184, 0.1)' }
                        }
                    },
                    plugins: {
                        legend: { labels: { color: '#E2E8F0' } }
                    }
                }
            });
        }
    }

    // ------------------------------------------------------------------------
    // 4. TRỢ LÝ ẢO 2D/3D-READY DUAL-MODE TIẾT KIỆM TOKEN (VIỆTBOT)
    // ------------------------------------------------------------------------
    function formatMarkdownSimple(text) {
        if (!text) return '';
        return text
            .replace(/```csharp([\s\S]*?)```/g, '<pre class="bg-black text-info p-2 rounded mt-1 mb-1 font-mono border border-info border-opacity-25" style="font-size:0.78rem;"><code>$1</code></pre>')
            .replace(/```([\s\S]*?)```/g, '<pre class="bg-black text-info p-2 rounded mt-1 mb-1 font-mono border border-info border-opacity-25" style="font-size:0.78rem;"><code>$1</code></pre>')
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            .replace(/`([^`]+)`/g, '<code class="hv-inline-code">$1</code>')
            .replace(/\n/g, '<br/>');
    }

    window.HoangVietMascot = {
        setMood: function (mood) {
            const dock = document.getElementById('globalAiMascotDock');
            const viewport = document.getElementById('widgetMascotRenderViewport');
            const mouth = document.getElementById('mascotMouthPath');

            if (dock) {
                dock.className = `ai-mascot-floating-dock mascot-mood-${mood || 'explaining'}`;
            }
            if (viewport) {
                viewport.setAttribute('data-current-mood', mood || 'explaining');
            }

            // [3D-UPGRADE-HOOK]: Nơi gắn trigger đổi animation 3D sau này
            if (window.HoangViet3DMascot && typeof window.HoangViet3DMascot.setWidgetMood === 'function') {
                window.HoangViet3DMascot.setWidgetMood(mood);
            }

            if (mouth) {
                if (mood === 'thinking') {
                    mouth.setAttribute('d', 'M 51 61 L 69 61');
                } else if (mood === 'interview') {
                    mouth.setAttribute('d', 'M 50 60 Q 60 64 70 60');
                } else {
                    mouth.setAttribute('d', 'M 50 59 Q 60 66 70 59');
                }
            }
        },

        toggleBubble: function (forceState) {
            const bubble = document.getElementById('mascotSpeechBubble');
            if (!bubble) return;
            if (typeof forceState === 'boolean') {
                bubble.classList.toggle('collapsed-bubble', !forceState);
            } else {
                bubble.classList.toggle('collapsed-bubble');
            }
        },

        askPreset: function (presetQuestion) {
            this.sendQuestion(presetQuestion);
        },

        askQuickQuestion: function (event) {
            if (event) event.preventDefault();
            const input = document.getElementById('mascotQuickInput');
            if (!input || !input.value.trim()) return;
            const q = input.value.trim();
            input.value = '';
            this.sendQuestion(q);
        },

        sendQuestion: async function (questionText) {
            const dock = document.getElementById('globalAiMascotDock');
            const bubbleText = document.getElementById('mascotBubbleText');

            this.toggleBubble(true);
            this.setMood('thinking');
            if (!bubbleText) return;

            const lessonCode = dock?.getAttribute('data-lesson-code') || '1.1';
            const lessonTitle = dock?.getAttribute('data-lesson-title') || '';
            const activeStepTitle = window.HoangVietFlow.getActiveStepTitle();

            bubbleText.innerHTML = '<span class="text-info"><i class="bi bi-hourglass-split me-1"></i>ViệtBot 2D đang tổng hợp câu trả lời...</span>';

            try {
                const res = await fetch('/AIStudio/AskMentor', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        message: questionText,
                        lessonCode: lessonCode,
                        lessonTitle: lessonTitle,
                        activeStepTitle: activeStepTitle
                    })
                });

                const data = await res.json();
                if (data && data.reply) {
                    this.setMood(data.mood || 'happy');
                    bubbleText.innerHTML = formatMarkdownSimple(data.reply);
                }
            } catch {
                this.setMood('explaining');
                bubbleText.innerHTML = 'Hãy kiểm tra kết nối hoặc bấm lại vào một trong 3 nút gợi ý nhanh nhé!';
            }
        }
    };

    // ------------------------------------------------------------------------
    // 5. HIỆU ỨNG INTERACTIVE SPOTLIGHT TRACKING TRÊN CÁC THẺ BENTO & SECTION
    // ------------------------------------------------------------------------
    function initSpotlightCards() {
        document.addEventListener('mousemove', function (e) {
            const cards = document.querySelectorAll('.hv-bento-card, .cp-section-card');
            cards.forEach(card => {
                const rect = card.getBoundingClientRect();
                if (
                    e.clientX >= rect.left - 100 &&
                    e.clientX <= rect.right + 100 &&
                    e.clientY >= rect.top - 100 &&
                    e.clientY <= rect.bottom + 100
                ) {
                    const x = e.clientX - rect.left;
                    const y = e.clientY - rect.top;
                    card.style.setProperty('--mouse-x', `${x}px`);
                    card.style.setProperty('--mouse-y', `${y}px`);
                }
            });
        }, { passive: true });
    }

    document.addEventListener('DOMContentLoaded', function () {
        const firstStepBtn = document.querySelector('#interactiveFlowChain .flow-step-box.active')
            || document.querySelector('#interactiveFlowChain .flow-step-box');
        if (firstStepBtn && window.HoangVietFlow) {
            window.HoangVietFlow.selectStep(firstStepBtn);
        }
        initHomeCharts();
        initSpotlightCards();
    });
})();
